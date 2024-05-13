using Microsoft.AspNetCore.Components.Authorization;
using SharedModels;
using SharedModels.DTOs;
using System.Security.Claims;

namespace ShopClient.Auth {
    public class CustomAuthStateProvider(AuthService authService) : AuthenticationStateProvider {
        private ClaimsPrincipal anonymus = new(new ClaimsIdentity());
        public async override Task<AuthenticationState> GetAuthenticationStateAsync() {
            try {
                var userSession = await authService.GetUserDetails();
                if(userSession==null || string.IsNullOrEmpty(userSession.Email))
                    return await Task.FromResult(new AuthenticationState(anonymus));
                var claimsPrincipal = authService.SetClaimPrincipal(userSession);
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            } catch (Exception){
                return await Task.FromResult(new AuthenticationState(anonymus));
            }
        }
        public async Task UpdateAuthenticationState(TokenProp tokenProp) {
            ClaimsPrincipal claimsPrincipal= new ();
            if (tokenProp != null && !string.IsNullOrEmpty(tokenProp!.Token)) {
                await authService.SetTokenToLocalStorage(JsonUtils.SerializeObject(tokenProp));
                UserSession userSession = await authService.GetUserDetails();
                if (userSession != null && !string.IsNullOrEmpty(userSession.Email)) {
                    claimsPrincipal = authService.SetClaimPrincipal(userSession);
                }
            } else {
                claimsPrincipal = anonymus;
                await authService.RemoveAccessTokenFromLocalStorage();
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
