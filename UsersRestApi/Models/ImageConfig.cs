using System.IO;

namespace UsersRestApi.Models
{
    public class ImageConfig
    {
        public string MainPath { get; set; } = string.Empty;
        public string ProductPath { get; set; } = string.Empty;
        public string PreviewPath { get; set; } = string.Empty;
        public string CollectionPath { get; set; } = string.Empty;
    }
}
