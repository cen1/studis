using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace studis.Models
{
    public static class Validate
    {
        public static bool veljavnoIme(string ime)
        {
            if (ime.Length < 1)
                return false;
            if (!Char.IsUpper(ime[0]))
                return false;
            foreach (char c in ime)
            {
                if (!Char.IsLetter(c))
                    return false;
            }
            return true;
        }

        //moski= 0, zenksi 1
        public static bool isEmso(string emso, DateTime datumRojstva, int spol)
        {
            //crc
            if (!isEmso(emso))
            {
                return false;
            }
            //ustreznost datuma
            DateTime emsoTime = new DateTime(1000 + Convert.ToInt32(emso.Substring(4, 3)), Convert.ToInt32(emso.Substring(2, 2)), Convert.ToInt32(emso.Substring(0, 2)));//year,month, day
            if (emsoTime.CompareTo(datumRojstva) != 0)
            {
                return false;
            }
            //ustreznost spola
            int stevilka= Convert.ToInt32(emso[9]);
            if (spol == 0)
            {
                if (stevilka >= 5)
                {
                    return false;
                }
            }
            else if (spol == 1)
            {
                if (stevilka < 5)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool isEmso(string emso)
        {
            Debug.Print("isemso");
            if (emso.Length != 13)
            {
                return false;
            }
            foreach (char c in emso)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            int p0 = Convert.ToInt32(emso[0]) * 7;
            int p1 = Convert.ToInt32(emso[1]) * 6;
            int p2 = Convert.ToInt32(emso[2]) * 5;
            int p3 = Convert.ToInt32(emso[3]) * 4;
            int p4 = Convert.ToInt32(emso[4]) * 3;
            int p5 = Convert.ToInt32(emso[5]) * 2;
            int p6 = Convert.ToInt32(emso[6]) * 7;
            int p7 = Convert.ToInt32(emso[7]) * 6;
            int p8 = Convert.ToInt32(emso[8]) * 5;
            int p9 = Convert.ToInt32(emso[9]) * 4;
            int p10 = Convert.ToInt32(emso[10]) * 3;
            int p11 = Convert.ToInt32(emso[11]) * 2;
            Debug.Print("emso12 " +Convert.ToInt32(emso[12]));
            int sum = p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11;
            int ostanek = sum % 11;
            Debug.Print("IZPISSS:  " + Convert.ToInt32(emso[12]) + " | " + ostanek);
            if ( (11 - ostanek) == Convert.ToInt32(emso[12]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}