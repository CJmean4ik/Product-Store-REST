using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Database.Entities
{
    [Table("Buyers")]
    public class BuyerEntity : BaseUserEntity
    {
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
