using System;
using System.Linq;
using studis.Models;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Security;

namespace studis.Models
{
    public class UserHelper
    {
        private studisEntities db;
        private static studisEntities baza = new studisEntities();

        public UserHelper()
        {
            db = new studisEntities();
        }

        public my_aspnet_users FindByName(String name)
        {
            return db.my_aspnet_users.Where(a => a.applicationId == 1).Where(b => b.name == name).FirstOrDefault();
        }

        public static profesor getProfByName(String name)
        {
            return baza.my_aspnet_users.Where(a => a.applicationId == 1).Where(b => b.name == name).FirstOrDefault().profesors.FirstOrDefault();
        }

        public static student GetStudentByUserName(String name)
        {
            return baza.my_aspnet_users.Where(a => a.applicationId == 1).Where(b => b.name == name).FirstOrDefault().students.FirstOrDefault();
        }

        public static student GetStudentByVpisna(int vpisna)
        {
            Debug.WriteLine("Vpisna: " + vpisna);
            return baza.students.FirstOrDefault(a => a.vpisnaStevilka == vpisna);
        }

        public my_aspnet_membership FindByEmail(String email)
        {
            return db.my_aspnet_membership.Where(b => b.Email == email).FirstOrDefault();
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

        public bool preveriPovprecje(student s)
        {
            vpi vp = s.vpis.Where(a => a.letnikStudija == 2).Last();
            if (vp == null) return false;

            double sum = 0;
            int cnt = 0;
            /*foreach (var o in vp.ocenas)
            {
                sum += o.ocena1;
                cnt++;
            }*/
            double avg = sum / cnt;
            if (avg >= 8) return true;
            else return false;

        }

        public bool imaZeton(student s)
        {
            var z = s.zetons.Where(a => a.porabljen == false);
            if (z.Count() > 0) return true;
            else return false;
        }

        public bool preveriZeton(student s, VpisniListModel vlm)
        {
            foreach (var z in s.zetons.Where(a => a.porabljen == false))
            {
                if (z.letnik == vlm.letnikStudija &&
                    z.oblikaStudija == vlm.oblikaStudija &&
                    z.studijskiProgram == vlm.studijskiProgram &&
                    z.vrstaStudija == vlm.vrstaStudija &&
                    z.vrstaVpisa == vlm.vrstaVpisa
                )
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> preveriLogiko(VpisniListModel vlm, student s, my_aspnet_users user, bool update)
        {
            List<string> napake = new List<string>();

            kandidat k = user.kandidats.FirstOrDefault();

            //prvi vpis
            if (!update)
            {
                if (s == null)
                {
                    if (k.ime != vlm.ime) napake.Add("Ime ni enako imenu v seznamu kandidatov");
                    if (k.priimek != vlm.priimek) napake.Add("Priimek ni enak imenu v seznamu kandidatov");
                    if (k.studijskiProgram != vlm.studijskiProgram) napake.Add("Študijski program ni enak programu v seznamu kandidatov");
                    if (k.email != vlm.email) napake.Add("Email ni enak tistemu v seznamu kandidatov");
                    if (vlm.letnikStudija != 1) napake.Add("Letnik ni enak 1");
                    if (vlm.vrstaVpisa != 1) napake.Add("izberete lahko le prvi vpis v letnik");
                }
                else
                {
                    //preveri kdaj je bil prvi vpis v ta letnik, ce je pravilen
                    if (vlm.studijskoLetoPrvegaVpisa > DateTime.Now.Year) napake.Add("Prvo leto vpisa je preveliko");

                    //preveri da je letnik enak ob ponavljanju ali vecji od zadnjega vpisa
                    int stponavljanj = 0;
                    var vpisi_sp = s.vpis.Where(a => a.studijskiProgram == vlm.studijskiProgram);
                    foreach (var v in vpisi_sp)
                    {
                        if (vlm.vrstaVpisa == 1)
                        {
                            //prvi vpis v letnik
                            if (vlm.letnikStudija == v.letnikStudija) napake.Add("Izbran je prvi vpis v letnik čeprav ste bili v ta letnik že vpisani");
                        }
                        if (vlm.vrstaVpisa == 2)
                        {
                            //ponavljanje
                            if (v.letnikStudija > vlm.letnikStudija) napake.Add("Ne morete ponavljati ker ste že bili vpisani v višji letnik");
                            if (v.letnikStudija == vlm.letnikStudija) stponavljanj++;
                        }
                    }
                    if (stponavljanj > 1) napake.Add("Samo enkrat lahko ponavljate isti letnik");
                    if (stponavljanj == 0 && vlm.vrstaVpisa == 2) napake.Add("Ne morete ponavljati letnika, ki ga še niste delali");

                    if (vlm.vrstaVpisa == 7 && (vpisi_sp.Last().letnikStudija != vlm.letnikStudija)) napake.Add("Letnik mora biti enak zadnjemu vpisu");
                    if (vlm.vrstaVpisa == 5 && (vlm.letnikStudija <= vpisi_sp.Last().letnikStudija)) napake.Add("Letnik mora biti večji od zadnjega vpisa");
                }
            }

            //preveri da se emso in datum rojstva ujemata
            string dan = vlm.emso.Substring(0, 2);
            string mesec = vlm.emso.Substring(2, 2);
            string leto = vlm.emso.Substring(4, 3);
            if (Convert.ToInt32(dan) != vlm.dr_dan ||
                Convert.ToInt32(mesec) != vlm.dr_mesec ||
                Convert.ToInt32(leto) != Convert.ToInt32(vlm.dr_leto.ToString().Substring(1, 3))
                )
            {
                napake.Add("Rojstni datum se ne ujema z EMŠO");
            }

            //preveri drzavo in obcino rojstva
            if (vlm.drzavaRojstva != 705 && vlm.obcinaRojstva != -1) napake.Add("Za občino rojstva izberite možnost TUJINA");
            if (vlm.drzava != 705 && vlm.obcina != -1) napake.Add("Za občino prebivanja izberite možnost TUJINA");
            if (vlm.drzavaZacasni != 705 && vlm.obcinaZacasni != -1 && vlm.drzavaZacasni != null) napake.Add("Za občino začasnega prebivališča izberite možnost TUJINA");

            //preveri letnik in studijski program
            if (vlm.studijskiProgram == 1000468 && vlm.letnikStudija > 3) napake.Add("Za ta program je maksimalen letnik 3");

            //preveri studijski program
            if (vlm.studijskiProgram != 1000468) napake.Add("Za ta program ne obstaja predmetnik in ni podprt");

            //preveri studijski program in klasius
            if (vlm.vrstaStudija != 16204) napake.Add("Izbira klasius/vrsta študija ni podprta");

            //blokiraj vpis 98
            if (user.my_aspnet_roles.Where(a => a.name == "Študent").FirstOrDefault() != null)
                if (vlm.vrstaVpisa == 98) napake.Add("Izbrana vrsta vpisa ni podprta");

            //preveri vrocanje
            if (vlm.vrocanje && vlm.vrocanjeZacasni) napake.Add("Izberite le en naslov za vročanje");
            if (!vlm.vrocanje && !vlm.vrocanjeZacasni) napake.Add("Izberite vsaj en naslov za vročanje");

            //preveri da so izpolnjeni vsi ali noben podatek o zacasnem
            if (vlm.naslovZacasni != null || vlm.obcinaZacasni != null || vlm.postnaStevilkaZacasni != null || vlm.drzavaZacasni != null || vlm.vrocanjeZacasni == true)
            {
                if (vlm.naslovZacasni == null || vlm.obcinaZacasni == null || vlm.postnaStevilkaZacasni == null || vlm.drzavaZacasni == null)
                {
                    napake.Add("Izpolnjeni morajo biti vsi podatki o začasnem prebivališču");
                }
            }

            if (!update)
            {
                //preveri ce res obstaja studijsko leto prvega vpisa oziroma če je trenutno če je prvi vpis
                if (s != null)
                {
                    var starvpis = s.vpis.Where(a => a.studijskiProgram == vlm.studijskiProgram).Where(b => b.letnikStudija == vlm.letnikStudija).FirstOrDefault();
                    if (starvpis != null) //obstaja vpis v ta letnik
                    {
                        if (vlm.studijskoLetoPrvegaVpisa != starvpis.studijskoLeto)
                        {
                            napake.Add("Študijsko leto prvega vpisa je napačno (nimate vpisa iz tistega leta)");
                        }
                    }
                    else //prvi vpis v letnik
                    {
                        if (vlm.studijskoLetoPrvegaVpisa != DateTime.Now.Year)
                        {
                            napake.Add("Študijsko leto prvega vpisa je lahko le letošnje");
                        }
                    }
                }
                else //kandidat
                {
                    if (vlm.studijskoLetoPrvegaVpisa != DateTime.Now.Year)
                    {
                        napake.Add("Študijsko leto prvega vpisa je lahko le letošnje");
                    }
                }
            }

            return napake;
        }

        public bool jePredmetnikVzpostavljen(vpi v)
        {
            if (v.vrstaVpisa != 1)
            {
                System.Diagnostics.Debug.WriteLine("Ponavljanje, zakljucek, pavziranje");
                return true; //ponavljanje, zakljucek, pavziranje
            }

            if (v.izvajanjes.Count() == 0)
            {
                System.Diagnostics.Debug.WriteLine("Vzpostavljen za " + v.id.ToString() + " je false cnt 0");
                return false;
            }
            else
            {
                //preveri kreditne
                PredmetHelper ph = new PredmetHelper();
                List<predmet> l = new List<predmet>();

                var izv = v.izvajanjes.ToList();
                foreach (var a in izv)
                {
                    l.Add(a.predmet);
                }

                if (ph.preveriKredite(l))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool jeDelniPredmetnikVzpostavljen(vpi v)
        {
            if (v.izvajanjes.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string linkZaPredmetnik(vpi v)
        {
            string r = "";
            if (v.letnikStudija == 1) r += "PrviPredmetnik";
            if (v.letnikStudija == 2) r += "DrugiPredmetnik";
            if (v.letnikStudija == 3) r += "TretjiPredmetnik";
            return r;
        }

        public static string DateToString(DateTime datum)
        {
            return datum.Day + "." + datum.Month + "." + datum.Year;
        }

        //dd.MM.yyyy
        public static DateTime StringToDate(string datum)
        {
            char[] delimiters = { '.', '/' };
            string[] stevilke = datum.Split(delimiters);
            DateTime date = new DateTime(Convert.ToInt32(stevilke[2]), Convert.ToInt32(stevilke[1]), Convert.ToInt32(stevilke[0]));

            return date;
        }

        public static int[] TimeToInts(DateTime cas)
        {
            int[] uramin = new int[2];
            uramin[0] = cas.Hour;
            uramin[1] = cas.Minute;
            return uramin;
        }

        public static DateTime IntsToTime(int ura, int min)
        {
            return new DateTime(1, 1, 1, ura, min, 0);
        }

        public static string TimeToString(DateTime cas)
        {
            if (cas.Hour != 0)
                return cas.Hour.ToString("00") + ":" + cas.Minute.ToString("00");
            else
                return "";
        }

        public void kreirajProfesorje()
        {
            foreach (var p in db.profesors.Where( a => a.id < 103).ToList())
            {
                string uname = p.ime.ToLower().Substring(0, 1) + p.priimek.ToLower().Substring(0, 1);
                if (p.id < 10)
                    uname += p.id.ToString().Substring(0, 1) + p.id.ToString().Substring(0, 1) + p.id.ToString().Substring(0, 1) + p.id.ToString().Substring(0, 1);
                else if (p.id < 100)
                    uname += p.id.ToString().Substring(0, 2) + p.id.ToString().Substring(0, 2);
                else
                    uname += p.id.ToString().Substring(0, 3) + p.id.ToString().Substring(0, 1);

                uname = uname.Replace("ž", "z");
                uname = uname.Replace("č", "c");
                uname = uname.Replace("š", "s");
                string pass = "testtest";
                string mail = uname + "@fri.uni-lj.si";

                System.Diagnostics.Debug.WriteLine("Dodajam: " + uname + " " + pass + " " + mail);

                /*MembershipUser user = Membership.CreateUser(uname, pass, mail);
                MembershipUser myObject = Membership.GetUser(uname);
                Roles.AddUserToRole(uname, "Profesor");
                string id = myObject.ProviderUserKey.ToString();*/

                try
                {
                    p.userId = Convert.ToInt32(db.my_aspnet_users.Where(a => a.name == uname).First().id);
                }
                catch (Exception e) { }
            }
            db.SaveChanges();
        }

        public void kreirajFiktivneRoke()
        {
            foreach (var r in db.izvajanjes.ToList())
            {
                izpitnirok ir = new izpitnirok();
                ir.fiktiven = true;
                ir.izvajanjeId = r.id;
                ir.prostorId = 1;
                ir.datum = Convert.ToDateTime("30.9.2015");
                db.izpitniroks.Add(ir);
            }
            db.SaveChanges();
        }
    }

}