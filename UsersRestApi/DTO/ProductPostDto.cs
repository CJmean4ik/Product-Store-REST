namespace UsersRestApi.DTO
{
    public class ProductPostDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CountOnStorage { get; set; }
        public IFormFile PreviewImage { get; set; }
        public List<IFormFile> Images { get; set; }
        public string SubCategory { get; set; } = string.Empty;
    }
}
