using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace studis.Models
{
    public class Sifrant
    {
        public int id { get; set; }
        public string naziv { get; set; }

        public Sifrant(int id, string naziv)
        {
            this.id = id;
            this.naziv = naziv;
        }

        public Sifrant(string naziv)
        {
            this.id = -1;
            this.naziv = naziv;
        }

        public override string ToString()
        {
            if(id != -1)
                return id + " - " + naziv;
            else 
                return "#" + " - " + naziv;
        }
    }
}