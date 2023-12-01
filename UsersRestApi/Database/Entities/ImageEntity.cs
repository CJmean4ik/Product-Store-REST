namespace UsersRestApi.Database.Entities
{
    public class ImageEntity
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }

        public ProductEntity ProductEntity { get; set; }
    }
}