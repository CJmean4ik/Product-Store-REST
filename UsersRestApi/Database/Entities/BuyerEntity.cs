using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Database.Entities
{
    [Table("Buyers")]
    public class BuyerEntity : BaseUserEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;

        public List<CartEntity> Carts { get; set; }
        public List<FavoritesEntity> Favourites { get; set; }
        public List<OrderEntity> Orders { get; set; }
    }
}
