namespace ProductAPI.DTO.Carts
{
    public class ProductCartsPostDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool InStock { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string PreviewName { get; set; }
    }
}
