using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace studis.Models
{
    public class StudijskoLeto
    {
        public int leto { get; set; }
        public string letoString
        {
            get { return this.ToString(); }
        }
        

        public StudijskoLeto(int leto) {
            this.leto= leto;
        }

        public StudijskoLeto(string leto)
        {
            this.leto = Convert.ToInt32(leto);
        }

        public string ToString()
        {
            return leto + "/" + (leto+1);
        }

        public int ToInt32()
        {
            return leto;
        }

        public StudijskoLeto next() {
            return new StudijskoLeto(leto+1);
        }

        public StudijskoLeto prev()
        {
            return new StudijskoLeto(leto - 1);
        }

        public static string toString(int leto)
        {
            return leto + "/" + (leto+1);
        }
    }
}