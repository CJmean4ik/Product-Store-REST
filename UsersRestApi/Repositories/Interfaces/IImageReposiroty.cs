namespace UsersRestApi.Repositories.Interfaces
{
    public interface IImageReposiroty<T, R, D>
    {      
        R Create(T image,string path, bool creatCopyIfExist = false);
        R Remove(string path);
    }
}
