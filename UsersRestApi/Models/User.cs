namespace UsersRestApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; }
        public string Salt { get; set; }
        public string HashPassword { get; set; }

    }
}
