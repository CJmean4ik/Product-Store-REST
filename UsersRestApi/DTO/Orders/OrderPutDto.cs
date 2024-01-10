namespace ProductAPI.DTO.Orders
{
    public class OrderPutDto
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string City { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string DeliveryType { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
