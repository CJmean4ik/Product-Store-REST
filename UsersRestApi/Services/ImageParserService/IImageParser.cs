using System.Threading.Tasks;

namespace UsersRestApi.Services.ImageParserService
{
    public interface IImageParser<T,R>
    {
        Task<R> GetImageAsync(string path);
        R CreateImage(T image, string path);
    }
}
