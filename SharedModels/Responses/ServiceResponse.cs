using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Responses {
    public record ServiceResponse(bool IsSuccess, string? Message) {// Immutable type
        public static ServiceResponse Null = new(false, "Model is null");
        public static ServiceResponse MainNameExists = new(false, "Name already exists");
        public static ServiceResponse Error = new(false, "Error occured in operation");
        
        public static ServiceResponse AlreadyRegistered = new(false, "User is registered");
        public static ServiceResponse RegistrationSuccessful= new(true, "User account created");

        public static ServiceResponse ProductRemoved = new(true, "Product removed successfully");


        public static ServiceResponse Saved = new(true, "Saved");
        public static ServiceResponse Updated = new(true, "Updated");
    }
}
