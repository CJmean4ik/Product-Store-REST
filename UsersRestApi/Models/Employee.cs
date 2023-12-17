using ProductAPI.Models;

namespace UsersRestApi.Entities
{
    public class Employee : UserBase
    {    
        public string Role { get; set; }
    }
}
