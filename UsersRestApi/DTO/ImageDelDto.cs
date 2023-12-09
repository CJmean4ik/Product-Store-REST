namespace ProductAPI.DTO
{
    public class ImageDelDto
    {
        public string? ProductName { get; set; }

        public bool IsPreviewDeletion { get; set; }
        public string? PreviewImageName { get; set; }

        public bool IsCollectionDeletion { get; set; }
        public string[]? AllImagesName { get; set; }
    }
}
