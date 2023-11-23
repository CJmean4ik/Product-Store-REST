namespace UsersRestApi.DTO
{
    public class ProductPostDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CountOnStorage { get; set; }
        public string SubCategory { get; set; } = string.Empty;
    }
}
