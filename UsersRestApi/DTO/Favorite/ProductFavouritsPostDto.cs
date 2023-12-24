namespace ProductAPI.DTO.Favourite
{
    public class ProductFavoritsPostDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string PreviewName { get; set; }
    }
}
