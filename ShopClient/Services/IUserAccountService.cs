using SharedModels.DTOs;
using SharedModels.Responses;

namespace ShopClient.Services {
    public interface IUserAccountService {
        Task<ServiceResponse> Register(UserDTO model);
        Task<LoginResponse> Login(LoginDTO model);
    }
}
