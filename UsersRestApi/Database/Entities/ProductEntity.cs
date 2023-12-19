using ProductAPI.Database.Entities;

namespace UsersRestApi.Database.Entities
{
    public class ProductEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CountOnStorage { get; set; }
        public DateTime DateCreated { get; set; }
        public string PreviewImageName { get; set; } = string.Empty;


        public int SubCategoryId { get; set; }
        public SubCategoryEntity SubCategory { get; set; }

        public List<ImageEntity> Images { get; set; }
        public List<CartEntity> Carts { get; set; }

    }
}
