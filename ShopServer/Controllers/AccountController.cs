using Microsoft.AspNetCore.Mvc;
using SharedModels.DTOs;
using SharedModels.Responses;
using ShopServer.Repository;

namespace ShopServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserAccount accountService) : Controller {//testy
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse>> Register(UserDTO model) {
            if(model == null)
                return BadRequest(ServiceResponse.Null);
            var response = await accountService.Register(model);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse>> Login(LoginDTO model) {
            if (model == null)
                return BadRequest(ServiceResponse.Null);
            var response = await accountService.Login(model);
            return Ok(response);
        }
        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo() {
            var token = GetTokenFromHeader();
            if(string.IsNullOrEmpty(token))
                return Unauthorized();
            UserSession user = await accountService.GetUserByToken(token);
            if (user == null || string.IsNullOrEmpty(user.Email))
                return Unauthorized();
            return Ok(user);
        }
        private string GetTokenFromHeader() {
            string Token = string.Empty;
            foreach(var header in Request.Headers) 
                if(header.Key == "Authorization") {
                    Token = header.Value.ToString();
                    break;
                }
            return Token.Split(" ").LastOrDefault()!;
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponse>> RefreshToken(PostRefreshTokenDTO model) {
            if (model == null)
                return Unauthorized();
            var result = await accountService.GetRefreshToken(model);
            return Ok(result);
        }

    }
}
