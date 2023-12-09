using UsersRestApi.Models;

namespace UsersRestApi.Repositories.Interfaces
{
    public interface IImageReposiroty<T, R>
    {
        Task<R> GetImageAsync(string path);
        R CreateImage(T image, string path);
        R CreateImages(List<T> image, string productName);
        R RemoveImageFile(string path);
        R RemoveImageDirectory(string path);
    }
}
