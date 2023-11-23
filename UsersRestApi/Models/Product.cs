namespace UsersRestApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CountOnStorage { get; set; }
        public DateTime DateCreated { get; set; }
        public Category Category { get; set; }
    }
}
