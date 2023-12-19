using ProductAPI.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersRestApi.Database.Entities
{
    [Table("Employees")]
    public class EmployeeEntity : BaseUserEntity
    {
    }
}
