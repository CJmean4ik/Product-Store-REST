namespace ProductAPI.DTO
{
    public class ImagePostDto
    {
        public int ProductId { get; set; }
        public IFormFile? Preview { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
