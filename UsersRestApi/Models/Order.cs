using UsersRestApi.Models;

namespace ProductAPI.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string City { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string DeliveryType { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;

        public Buyer Buyer { get; set; }

        public List<Product> Products { get; set; }
    }
}
