using ProductAPI.Database.Entities;
using UsersRestApi.Database.Entities;
using UsersRestApi.Entities;
using UsersRestApi.Services.Password;

namespace UsersRestApi.Services.PasswordHasherService
{
    public class PasswordHasher : IPasswordHasher<BaseUserEntity>
    {
        public bool Decryption(string enteredPassword, string salt, string hashPassword)
        {
            var res = BCrypt.Net.BCrypt.Verify(enteredPassword,hashPassword);
            return res;
        }

        public void Encryption(string enteredPassword, BaseUserEntity user)
        {
            string SALT = BCrypt.Net.BCrypt.GenerateSalt();
            string HASH_PASSWORD = BCrypt.Net.BCrypt.HashPassword(enteredPassword, SALT);
            user.Salt = SALT;
            user.HashPassword = HASH_PASSWORD;
        }

    }
}
