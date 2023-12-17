using UsersRestApi.Models;

namespace ProductAPI.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public Product Product { get; set; }
        public Buyer Buyer { get; set; }
    }
}
