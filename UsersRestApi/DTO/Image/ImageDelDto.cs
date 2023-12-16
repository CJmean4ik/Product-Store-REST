namespace ProductAPI.DTO.Image
{
    public class ImageDelDto
    {
        public int? ProductId { get; set; }
        public List<ImageParamsDelDto> ParamsDelDtos { get; set; } = new List<ImageParamsDelDto>();
    }
}
