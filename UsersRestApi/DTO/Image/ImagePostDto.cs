namespace ProductAPI.DTO.Image
{
    public class ImagePostDto
    {
        public int ProductId { get; set; }
        public IFormFile? Preview { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

        public bool ReplaceImageIfExist { get; set; } = default;
    }
}
