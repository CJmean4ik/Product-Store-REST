namespace ProductAPI.Models
{
    public class Buyer : UserBase
    {
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
