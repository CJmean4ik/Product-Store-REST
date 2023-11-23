namespace UsersRestApi.Database.Entities
{
    public class SubCategoryEntity
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
