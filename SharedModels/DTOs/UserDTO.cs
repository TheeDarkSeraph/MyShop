using System.ComponentModel.DataAnnotations;

namespace SharedModels.DTOs {
    public class UserDTO {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;


    }
}
