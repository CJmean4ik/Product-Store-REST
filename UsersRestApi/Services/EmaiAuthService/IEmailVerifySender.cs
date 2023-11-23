namespace UsersRestApi.Services.EmaiAuthService
{
    public interface IEmailVerifySender
    {
        public int Code { get; set; }
        void GenerateCode();
        void SendCode(string toAddress);
        bool VerifyCode(string code,string verifyCode);
    }
}
