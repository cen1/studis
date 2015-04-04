using System;
using System.Linq;
using studis.Models;
using System.Security.Cryptography;
using System.Net.Mail;

namespace studis.Models
{
    public class User
    {

        public static my_aspnet_users FindByName(studisEntities db, String name)
        {
            var L2EQuery = db.my_aspnet_users.Where(a => a.applicationId == 1).Where(b => b.name == name);
            var user = L2EQuery.FirstOrDefault<my_aspnet_users>();
            return user;
        }

        public static my_aspnet_membership FindByEmail(studisEntities db, String email)
        {
            var L2EQuery = db.my_aspnet_membership.Where(b => b.Email == email);
            var m = L2EQuery.FirstOrDefault<my_aspnet_membership>();
            return m;
        }

        public static string GeneratePasswordResetToken(int id)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                Random rnd = new Random();
                string data = DateTime.Now.ToString() + id.ToString() + rnd.Next(1, Int32.MaxValue);
                var hash = sha1.ComputeHash(System.Text.Encoding.ASCII.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }

        public static void SendEmail(string text, string to)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("skrbnik.fri@gmail.com");
            msg.To.Add(new MailAddress(to));
            msg.Subject = "Pozabljeno geslo";

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("skrbnik.fri@gmail.com", "gviUaqLMEPFelTamEos6");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
    }

}