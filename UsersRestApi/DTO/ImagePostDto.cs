namespace ProductAPI.DTO
{
    public class ImagePostDto
    {
        public string ProductName { get; set; }
        public IFormFile PreviewImage { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
