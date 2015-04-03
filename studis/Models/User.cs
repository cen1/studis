using System;
using System.Linq;
using studis.Models;

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
    }

}