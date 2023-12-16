namespace ProductAPI.DTO.Image
{
    public class ImagePutDto
    {
        public int ProductId { get; set; }
        public bool IsPreviewUpdating { get; set; }
        public string? OldImageName { get; set; }
        public IFormFile? NewImage { get; set; }

    }
}
