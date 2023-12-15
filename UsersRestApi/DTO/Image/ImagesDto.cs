namespace ProductAPI.DTO.Image
{
    public class ImageDto
    {
        public string OldImageName { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
