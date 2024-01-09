namespace ProductAPI.DTO.Orders
{
    public class OrderPostDto 
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string DeliveryType { get; set; } = string.Empty;
        public int TotalPrice { get; set; }

        public Dictionary<int, int> ProductsCount { get; set; } = new Dictionary<int, int>();
    }
}
