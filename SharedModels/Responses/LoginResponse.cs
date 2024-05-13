using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Responses {
    public record LoginResponse (bool Flag, string? Message, string Token = null!, string RefreshToken = null!) {
        public static LoginResponse Null = new(false, "Model is null");
        public static LoginResponse UserNotFound = new(false, "User not found");
        public static LoginResponse InvalidLogin = new(true, "Invalid username/password");
        public static LoginResponse Error = new(false, "Error occured in operation");
    }
}
