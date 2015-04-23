using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace studis.Models
{
    public class ZetonModel
    {
        public studisEntities db = new studisEntities();

        [Required]
        [Display(Name = "Žeton za letnik študija")]
        public int letnik { get; set; }

        [Required]
        public int vpisna { get; set; }


        public List<Sifrant> pridobiLetnike(int id)
        {
            //pripravi seznam dovoljenih letnikov
            var vls = db.vpis.Where(a => a.vpisnaStevilka == id);
            //dovoljen je vpis +1 ali max ce se ne obstaja
            int max = 0;
            int ponavljal = 0;
            foreach (var vpis in vls)
            {
                if (vpis.letnikStudija > max) max = vpis.letnikStudija;
                else if (vpis.letnikStudija == max) ponavljal = max;
            }
            bool lahkoPonavlja = true;
            if (ponavljal == max) lahkoPonavlja = false;

            List<Sifrant> letniki = new List<Sifrant>();
            if (lahkoPonavlja) {
                letniki.Add(new Sifrant(ponavljal, "Ponavljanje letnika"));
                letniki.Add(new Sifrant(max, "Naslednji letnik"));
            }
            else {
                letniki.Add(new Sifrant(max, "Naslednji letnik"));
            }

            return letniki;
        }
    }
}