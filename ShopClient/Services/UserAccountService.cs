using SharedModels;
using SharedModels.DTOs;
using SharedModels.Responses;
using System.Net.Http.Json;

namespace ShopClient.Services {
    public class UserAccountService(HttpClient httpClient) : IUserAccountService {
        private const string AccountBaseUrl = "api/account";
        public async Task<LoginResponse> Login(LoginDTO model) {
            var response = await httpClient.PostAsync($"{AccountBaseUrl}/login", JsonUtils.GenerateStringContent(JsonUtils.SerializeObject(model)));
            if(!response.IsSuccessStatusCode)
                return LoginResponse.Error;
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonUtils.DeserializeJsonString<LoginResponse>(apiResponse);
        }

        public async Task<ServiceResponse> Register(UserDTO model) {
            var response = await httpClient.PostAsync($"{AccountBaseUrl}/register", JsonUtils.GenerateStringContent(JsonUtils.SerializeObject(model)));
            if (!response.IsSuccessStatusCode)
                return ServiceResponse.Error;
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonUtils.DeserializeJsonString<ServiceResponse>(apiResponse);
        }
    }
}
