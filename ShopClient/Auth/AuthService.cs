using Blazored.LocalStorage;
using Microsoft.AspNetCore.WebUtilities;
using SharedModels;
using SharedModels.DTOs;
using SharedModels.Responses;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ShopClient.Auth {
    public class AuthService(ILocalStorageService localStorageService, HttpClient httpClient) {
        public ClaimsPrincipal SetClaimPrincipal(UserSession model) => new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim> {
                    new Claim(ClaimTypes.Name, model.Name),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, ((int)model.Role).ToString()),// regular tostring will turn it into the enum's name
                }, "AccessTokenAuth"));

        public async Task<UserSession> GetUserDetails() {
            string token = await GetAccessTokenFromLocalStorage();
            if (string.IsNullOrEmpty(token))
                return null!;
            await AddHeaderToHttpClient();
            var userDetails = JsonUtils.DeserializeJsonString<TokenProp>(token);
            if (userDetails == null || string.IsNullOrEmpty(userDetails.Token))
                return null!;
            // we now have a token
            var response = await GetUserDetailsFromApi(); // get user details, auth header is added
            if (response.IsSuccessStatusCode)
                return await GetUserSession(response);
            if (httpClient.DefaultRequestHeaders.Contains("Authorization")
                && response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                var encodedToken = Encoding.UTF8.GetBytes(userDetails.RefreshToken!);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                var model = new PostRefreshTokenDTO { RefreshToken = validToken };
                var refreshResponse = await httpClient.PostAsync("api/account/refresh-token",
                                       JsonUtils.GenerateStringContent(JsonUtils.SerializeObject(model)));
                if (refreshResponse.IsSuccessStatusCode && refreshResponse.StatusCode == System.Net.HttpStatusCode.OK) {
                    var apiResponse = await refreshResponse.Content.ReadAsStringAsync();
                    var loginResponse = JsonUtils.DeserializeJsonString<LoginResponse>(apiResponse);
                    await SetTokenToLocalStorage(JsonUtils.SerializeObject(new TokenProp() { Token = loginResponse.Token, RefreshToken = loginResponse.RefreshToken }));
                    var callApiAgain = await GetUserDetailsFromApi();
                    if(callApiAgain.IsSuccessStatusCode)
                        return await GetUserSession(callApiAgain);
                }
            }
            return null!;
        }
        async Task<string> GetAccessTokenFromLocalStorage() => (await localStorageService.GetItemAsync<string>("access_token"))!;
        async Task AddHeaderToHttpClient() {
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                JsonUtils.DeserializeJsonString<TokenProp>(await GetAccessTokenFromLocalStorage()).Token);
        }
        // Auth Header is expexted to have been added
        async Task<HttpResponseMessage> GetUserDetailsFromApi() => await httpClient.GetAsync("api/account/user-info");
        public static async Task<UserSession> GetUserSession(HttpResponseMessage response)
            => JsonUtils.DeserializeJsonString<UserSession>(await response.Content.ReadAsStringAsync());
        public async Task SetTokenToLocalStorage(string token)
            => await localStorageService.SetItemAsStringAsync("access_token", token);
        public async Task RemoveAccessTokenFromLocalStorage() => await localStorageService.RemoveItemAsync("access_token");

    }
}
