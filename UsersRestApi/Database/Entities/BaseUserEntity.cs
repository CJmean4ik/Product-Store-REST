namespace ProductAPI.Database.Entities
{
    public class BaseUserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;

    }
}
