using UsersRestApi.Models;

namespace UsersRestApi.Repositories.Interfaces
{
    public interface IImageReposiroty<T, R>
    {
        Task<R> GetImageAsync(string path);
        R CreateImage(T image, string path);
        R RemoveImage(string path);
        R UpdateImage(string path);
    }
}
