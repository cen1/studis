using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace studis.Models
{
    public static class Sifranti
    {
        public static readonly Sifrant[] KLASIUS = {
            new Sifrant(12001, "Osnovnošolska izobrazba"),
            new Sifrant(13001, "Nižja poklicna izobrazba"),
            new Sifrant(14001, "Srednja poklicna izobrazba"),
            new Sifrant(15001, "Srednja strokovna izobrazba"),
            new Sifrant(15001, "Srednja splošna izobrazba"),
            new Sifrant(16102, "Višješolska izobrazba (predbolonjska)"),
            new Sifrant(16201, "Specialializacija po visokošolski izobrazbi (predbolonjska)"),
            new Sifrant(16202, "Visokošolska strokovna izobrazba (predbolonjska)"),
            new Sifrant(17001, "Specialializacija po visokošolski strokovni izobrazbi (predbolonjska)"),
            new Sifrant(17002, "Visokošolska univerzitetna izobrazba (predbolonjska)"),
            new Sifrant(18101, "Specialializacija po univerzitetni izobrazbi (predbolonjska)"),
            new Sifrant(18102, "Magisterij znanosti (predbolonjska)"),
            new Sifrant(18201, "Doktorat znanosti (predbolonjska)"),
            new Sifrant(16101, "Višja strokovna izobrazba"),
            new Sifrant(16203, "Visokošolska strokovna izobrazba (prva bolonjska stopnja)"),
            new Sifrant(16204, "Visokošolska univerzitetna izobrazba (prva bolonjska stopnja)"),
            new Sifrant(17003, "Magistrska izobrazba (druga bolonjska stopnja)"),
            new Sifrant(18202, "Doktorat znanosti (tretja bolonjska stopnja)"),
        };

        public static readonly Sifrant[] VRSTAVPISA = {
            new Sifrant(01, "Prvi vpis v letnik/dodatno leto"),
            new Sifrant(02, "Ponavljanje letnika"),
            new Sifrant(03, "Nadaljevanje letnika"),
            new Sifrant(04, "Podaljšanje statusa študenta"),
            new Sifrant(05, "Vpis po merilih za prehode v višji letnik"),
            new Sifrant(06, "Vpis v semester skupnega št. programa"),
            new Sifrant(07, "Vpis po merilih za prehode v isti letnik"),
            new Sifrant(98, "Vpis za zaključek"),
        };

        public static readonly Sifrant[] NACINSTUDIJA = {
            new Sifrant(1, "redni"),
            new Sifrant(3, "izredni"),
        };

        public static readonly Sifrant[] OBLIKASTUDIJA = {
            new Sifrant(1, "na lokaciji"),
            new Sifrant(2, "na daljavo"),
            new Sifrant(3, "e-študij"),
        };

        public static readonly Sifrant[] STUDIJSKIPROGRAM = {
            new Sifrant(     -1, "Humanistika in družb. - DR III"),
            new Sifrant(1000479, "INF. SISTEMI IN ODLOČANJE - DR"),
            new Sifrant(1000480, "INFORMAC. SISTEMI IN ODLOČANJE"),
            new Sifrant(1000472, "Kognitivna znanaost MAG II. st."),
            new Sifrant(1001001, "Multimedija UN 1.st."),
            new Sifrant(1000977, "PEDAGOŠKO RAČ. IN INF. MAG-II. st."),
            new Sifrant(     -1, "Predmetnik za tuje študente na izmenjavi"),
            new Sifrant(1000475, "RAČUNAL. IN INFORMATIKA UN"),
            new Sifrant(1000425, "RAČUNAL. IN MATEMATIKA UN"),
            new Sifrant(1000477, "RAČUNAL. IN INFORMATIKA VS"),
            new Sifrant(1000407, "RAČUNAL. IN MATEMA. UN-I. ST."),
            new Sifrant(1000471, "RAČUNALN. IN INFORM. MAG II. ST."),
            new Sifrant(1000468, "RAČUNALN. IN INFORM. UN-I. ST."),
            new Sifrant(1000470, "RAČUNALN. IN INFORM. VS-I. ST."),
            new Sifrant(1000474, "RAČUNALNIŠ. IN INF. DR-III. ST."),
            new Sifrant(1000478, "RAČUNALNIŠTVO IN INF. - DR"),
            new Sifrant(1000481, "RAČUNALNIŠTVO IN INF. - MAG"),
            new Sifrant(     -1, "RAČUNALNIŠTVO IN INF. - VIS"),
            new Sifrant(     -1, "RAČUNALNIŠTVO IN INF. - VŠ"),
            new Sifrant(1000934, "Računalništvo in matematika MAG II. st."),
            new Sifrant(1000469, "Upravna infromatika UN 1. st"),    
        };

    }
}