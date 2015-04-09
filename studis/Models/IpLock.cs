using System;
using System.Linq;
using studis.Models;

namespace studis.Models
{
    public class IpLock
    {
        public static studisEntities db = new studisEntities();

        public static ip_lock FindActiveByIp(String ip)
        {
            DateTime cet = UserHelper.TimeCET();
            var L2EQuery = db.ip_lock.Where(a => a.ip == ip).Where(b => b.locked_until > cet);
            var ipl = L2EQuery.FirstOrDefault<ip_lock>();
            return ipl;
        }

        public static void Add(ip_lock ipl)
        {
            db.ip_lock.Add(ipl);
            db.SaveChanges();
        }
    }

}