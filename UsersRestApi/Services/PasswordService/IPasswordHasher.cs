namespace UsersRestApi.Services.Password
{
    public interface IPasswordHasher<T> where T : class
    {
        void Encryption(string enteredPassword, T user);
        bool Decryption(string enteredPassword, string salt, string hashPassword);
    }
}
