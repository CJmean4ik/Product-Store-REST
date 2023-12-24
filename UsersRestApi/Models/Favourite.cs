namespace ProductAPI.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string PreviewName { get; set; }
    }
}
