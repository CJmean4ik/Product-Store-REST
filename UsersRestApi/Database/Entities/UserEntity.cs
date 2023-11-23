namespace UsersRestApi.Database.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Salt { get; set; }
        public string HashPassword { get; set; }

    }
}
