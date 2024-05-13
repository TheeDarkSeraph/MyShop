using System.ComponentModel.DataAnnotations;

namespace SharedModels.DTOs {
    public class LoginDTO {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
