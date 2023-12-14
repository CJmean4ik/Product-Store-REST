using UsersRestApi.Models;

namespace UsersRestApi.Repositories.Interfaces
{
    public interface IImageReposiroty<T, R, D>
    {      
        R CreateImage(T image);
        R RemoveImageFile(string path);
    }
}
