namespace ProductAPI.DTO.Image
{
    public class ImageDelDto
    {
        public int? ProductId { get; set; }
        public List<string> ImageNames { get; set; } = new List<string>();
    }
}
