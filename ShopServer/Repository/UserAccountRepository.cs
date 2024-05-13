using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SharedModels.DTOs;
using SharedModels.Responses;
using ShopServer.Data;
using ShopServer.Models;
using System.Security.Cryptography;
using System.Text;

namespace ShopServer.Repository {
    public class UserAccountRepository(ShopDBContext context): IUserAccount {
        public async Task<ServiceResponse> Register(UserDTO model) {
            if (model == null)
                return ServiceResponse.Null;

            if (context.Users.FirstOrDefault(_ => _.Email.ToLower().Equals(model.Email.ToLower())) != null)
                return ServiceResponse.AlreadyRegistered;

            var newUser = new UserAccount {
                Name = model.Name,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password) // user role is user by default
            };
            if (context.Users.Count() == 0) // first user is admin by default, a fast way to create admin user
                newUser.Role = UserRole.Admin;

            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            return ServiceResponse.Saved;
        }
        [HttpPost("Login")]
        public async Task<LoginResponse> Login(LoginDTO model) {
            if (model == null)
                return LoginResponse.Null;
            var user = await context.Users.FirstOrDefaultAsync(_ => _.Email.ToLower().Equals(model.Email.ToLower()));
            if (user == null)
                return LoginResponse.UserNotFound;
            if(!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return LoginResponse.InvalidLogin;
            var (accessToken, refreshToken) = await GenerateTokens();
            await SaveOrUpdateTokenInfo(user.Id, accessToken, refreshToken);
            return new LoginResponse(true, "Login successful", accessToken, refreshToken);
        }
        // I think it is meaningless to have an access token larger than refresh token, if both need to be unique in their own column
        #region Generate Unique Access & Refresh Tokens
        private async Task<(string AccessToken, string RefreshToken)> GenerateTokens() {
            string accessToken = await GenerateUniqueAccessToken(256);
            string refreshToken = await GenerateUniqueRefreshToken(64);
            return (accessToken, refreshToken);
        }
        private async Task<string> GenerateUniqueAccessToken(int numOfBytes) {
            var token = GenerateToken(numOfBytes);
            while (await context.Tokens.AsNoTracking().FirstOrDefaultAsync(_ => _.AccessToken == token) != null)
                token = GenerateToken(numOfBytes);
            return token;
        }
        private async Task<string> GenerateUniqueRefreshToken(int numOfBytes) {
            var token = GenerateToken(numOfBytes);
            while (await context.Tokens.AsNoTracking().FirstOrDefaultAsync(_ => _.RefreshToken == token) != null) 
                token = GenerateToken(numOfBytes);
            return token;
        }
        private static string GenerateToken(int numOfBytes) =>
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(numOfBytes));

        #endregion

        private async Task SaveOrUpdateTokenInfo(int userId, string accessToken, string refreshToken) {
            var userTokenInfo = await context.Tokens.FirstOrDefaultAsync(_ => _.UserId == userId);
            if (userTokenInfo == null) {
                context.Tokens.Add(new TokenInfo { UserId = userId, AccessToken = accessToken, RefreshToken = refreshToken });
                await context.SaveChangesAsync();
            } else {
                userTokenInfo.RefreshToken = refreshToken;
                userTokenInfo.AccessToken = accessToken;
                userTokenInfo.ExpiryDate = DateTime.Now.AddDays(1);
                await context.SaveChangesAsync();
            }
        }
        public async Task<UserSession> GetUserByToken(string token) {
            var result = await context.Tokens.FirstOrDefaultAsync(_ => _.AccessToken == token);
            if (result == null)
                return null!;
            
            var userInfo = await context.Users.FirstOrDefaultAsync(_ => _.Id == result.UserId);
            if(userInfo == null || result.ExpiryDate < DateTime.Now)
                return null!;

            return new UserSession() {
                Name = userInfo.Name,
                Email = userInfo.Email,
                Role = userInfo.Role
            };
        }

        public async Task<LoginResponse> GetRefreshToken(PostRefreshTokenDTO model) {
            var decodeToken = WebEncoders.Base64UrlDecode(model.RefreshToken);
            string normalToken = Encoding.UTF8.GetString(decodeToken);
            
            var getToken = context.Tokens.FirstOrDefault(_ => _.RefreshToken == normalToken);
            if (getToken == null)
                return LoginResponse.Null;

            var (newAccessToken, newRefreshToken) = await GenerateTokens();
            await SaveOrUpdateTokenInfo(getToken.UserId, newAccessToken, newRefreshToken);

            return new LoginResponse(true, "Token refreshed", newAccessToken, newRefreshToken);
        }
    }
}
