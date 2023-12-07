using System.IO;

namespace UsersRestApi.Models
{
    public class ImageConfig
    {
        public string MainPath { get; set; } = string.Empty;
        public string ProductPath { get; set; } = string.Empty;
        public string ProductPreviewPath { get; set; } = string.Empty;
        public string ProductImagesPath { get; set; } = string.Empty;

        public bool CreateProductDirectory(string productName)
        {
            var productPath = ProductPath.Replace("FOR_RAPLACE", productName);
            return CreateDirectory(productPath);
        }
        public bool CreatePreviewDirectory(string productName)
        {
            var productPath = ProductPath.Replace("FOR_RAPLACE", productName);
            return CreateDirectory(productPath + "//Preview");
        }
        public bool CreateImageDirectory(string productName)
        {
            var productPath = ProductPath.Replace("FOR_RAPLACE", productName);
            return CreateDirectory(productPath + "//Images");
        }
        private bool CreateDirectory(string path)
        {
            if (Directory.Exists(path))
                return false;

            Directory.CreateDirectory(path);
            return true;
        }

        public bool RemoveProductDirectory(string productName)
        {
            var productPath = ProductPath.Replace("FOR_RAPLACE", productName);
            return RemoveDirecotry(productPath);
        }       
        private bool RemoveDirecotry(string path)
        {
            if (!Directory.Exists(path))
                return false;

            Directory.Delete(path);
            return true;
        }
    }
}
