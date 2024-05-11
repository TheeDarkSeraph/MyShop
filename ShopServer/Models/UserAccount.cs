namespace ShopServer.Models {
    public enum UserRole {
        Admin,
        User
    }
    public class UserAccount {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.User;
    }
}
