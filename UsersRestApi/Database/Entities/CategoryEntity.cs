namespace UsersRestApi.Database.Entities
{
    public class CategoryEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public List<SubCategoryEntity> SubCategories { get; set; }
    }
}
