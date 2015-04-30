using System;
using System.Linq;
using studis.Models;

namespace studis.Models
{
    public class IpLock
    {
        private studisEntities db;

        public IpLock()
        {
            db = new studisEntities();
        }

        public ip_lock FindActiveByIp(String ip)
        {
            DateTime cet = UserHelper.TimeCET();
            var L2EQuery = db.ip_lock.Where(a => a.ip == ip).Where(b => b.locked_until > cet);
            var ipl = L2EQuery.FirstOrDefault<ip_lock>();
            return ipl;
        }

        public void Add(ip_lock ipl)
        {
            db.ip_lock.Add(ipl);
            db.SaveChanges();
        }
    }

}