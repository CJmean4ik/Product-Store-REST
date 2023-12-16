using UsersRestApi.Models;

namespace UsersRestApi.Repositories.Interfaces
{
    public interface IImageReposiroty<T, R, D>
    {      
        R CreateImage(T image,string path, bool creatCopyIfExist = false);
        R RemoveImageFile(string path);
    }
}
