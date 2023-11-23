using System.Net;
using System.Net.Mail;

namespace UsersRestApi.Services.EmaiAuthService
{
    public class EmailVerifySender : IEmailVerifySender
    {
        public int Code { get; set; }
        public void GenerateCode()
        {
            Code = new Random().Next(0, 10000);
        }
        public void SendCode(string toAddress)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("userverify@gmail.com");
                mailMessage.To.Add(toAddress);
                mailMessage.Subject = "Verify cod for sign up";
                mailMessage.Body = "" + Code;

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("gromovstas64@gmail.com", "tzgu okmr lctm uojl");
                    smtpClient.EnableSsl = true;
                    try
                    {
                        smtpClient.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        smtpClient.Dispose();
                    }
                }
                }
            }
        public bool VerifyCode(string code,string verifyCode)
        {        
            return verifyCode == code ? true : false;
        }
    }
}
