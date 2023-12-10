namespace ProductAPI.DTO
{
    public class ImagePutDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public string OldPreviewName { get; set; } = string.Empty;
        public IFormFile NewPreview { get; set; }



    }
}
