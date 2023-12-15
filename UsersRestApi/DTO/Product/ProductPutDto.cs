namespace ProductAPI.DTO.Product
{
    public class ProductPutDto
    {
        public int TransportId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CountOnStorage { get; set; }
        public string PreviewImage { get; set; } = string.Empty;
        public string SubCategory { get; set; } = string.Empty;
    }
}
