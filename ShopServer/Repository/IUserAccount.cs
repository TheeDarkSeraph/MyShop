using SharedModels.DTOs;
using SharedModels.Responses;

namespace ShopServer.Repository {
    public interface IUserAccount {
        Task<ServiceResponse> Register(UserDTO model);
        Task<LoginResponse> Login(LoginDTO model);
        Task<UserSession> GetUserByToken(string token);
        Task<LoginResponse> GetRefreshToken(PostRefreshTokenDTO model);
    }
}
