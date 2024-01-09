namespace ProductAPI.Database.Entities
{
    public class OrderEntity
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string City { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string DeliveryType { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;

        public int BuyerId { get; set; }
        public BuyerEntity Buyer { get; set; }

        public List<OrderProductEntity> OrderProducts { get; set; }
    }
}
