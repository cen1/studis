using System;
using System.Linq;
using studis.Models;
using System.Security.Cryptography;
using System.Net.Mail;

namespace studis.Models
{
    public class UserHelper
    {
        public static studisEntities db = new studisEntities();

        public static my_aspnet_users FindByName(String name)
        {
            var L2EQuery = db.my_aspnet_users.Where(a => a.applicationId == 1).Where(b => b.name == name);
            var user = L2EQuery.FirstOrDefault<my_aspnet_users>();
            return user;
        }

        public static my_aspnet_membership FindByEmail(String email)
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
                string hex = BitConverter.ToString(hash);
                hex = hex.Replace("-", "");
                return hex;
            }
        }

        public static void SendEmail(string text, string to)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("studis.fri@gmail.com");
            msg.To.Add(new MailAddress(to));
            msg.IsBodyHtml = true;
            msg.Subject = "Pozabljeno geslo";
            msg.Body = text;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("studis.fri@gmail.com", "gviUaqLMEPFelTamEos6");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }

        public static DateTime TimeCET() {
            return TimeZoneInfo.ConvertTime (DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
        }

        public static DateTime TimeCET(DateTime t)
        {
            return TimeZoneInfo.ConvertTime(t, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
        }

        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static bool preveriPovprecje(student s)
        {
            vpi vp = s.vpis.Where(a => a.letnikStudija == 2).Last();
            double sum = 0;
            int cnt = 0;
            foreach (var o in vp.ocenas)
            {
                sum += o.ocena1;
                cnt++;
            }
            double avg = sum / cnt;
            if (avg >= 8) return true;
            else return false;

        }

        public static bool imaZeton(student s)
        {
            var z = s.zetons.Where(a => a.porabljen == false);
            if (z.Count() > 0) return true;
            else return false;
        }
    }

}