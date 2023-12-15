namespace ProductAPI.DTO.Image
{
    public class ImagePutDto
    {
        public int ProductId { get; set; }
        public List<ImageDto> Images { get; set; }
    }
}
