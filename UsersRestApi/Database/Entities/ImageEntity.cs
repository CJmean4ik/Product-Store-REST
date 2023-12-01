namespace UsersRestApi.Database.Entities
{
    public class ImageEntity
    {
        public int ImageId { get; set; }
        public byte[] Image { get; set; }

        public int ProductId { get; set; }
        public ProductEntity ProductEntity { get; set; }
    }
}
