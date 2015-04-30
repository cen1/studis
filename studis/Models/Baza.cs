using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using studis.Models;

namespace studis.Models
{
    public static class Baza
    {
        
        public static VpisniListModel getVpisniList(int id)
        {
            studisEntities db = new studisEntities();
            var vpi = db.vpis.SingleOrDefault(v => v.id == id);
            var student = vpi.student;

            VpisniListModel vpisniList = new VpisniListModel();
            vpisniList.ime = student.ime;
            vpisniList.priimek = student.priimek;
            vpisniList.prenosniTelefon = student.prenosniTelefon;
            vpisniList.datumRojstva = student.datumRojstva;
            vpisniList.davcnaStevilka = student.davcnaStevilka ?? default(int);
            vpisniList.emso = student.emso;
            vpisniList.email = student.email;
            vpisniList.krajRojstva = student.krajRojstva;
            vpisniList.obcinaRojstva = student.obcinaRojstva;
            vpisniList.drzavaRojstva = student.drzavaRojstva;
            vpisniList.drzavljanstvo = student.drzavljanstvo;
            vpisniList.naslov = student.naslov;
            vpisniList.obcina = student.obcina;
            vpisniList.vrocanje = student.vrocanje ?? default(bool);
            vpisniList.naslov = student.naslovZacasni;
            vpisniList.obcina = student.obcinaZacasni ?? default(int);
            vpisniList.vrocanjeZacasni = student.vrocanjeZacasni ?? default(bool);
            vpisniList.studijskiProgram = vpi.studijskiProgram;
            vpisniList.smer = vpi.smer ?? default(int);
            vpisniList.krajIzvajanja = vpi.krajIzvajanja;
            vpisniList.izbirnaSkupina = vpi.izbirnaSkupina;
            vpisniList.studijskiProgram2 = vpi.studijskiProgram2 ?? default(int);
            vpisniList.smer2 = vpi.smer2 ?? default(int);
            vpisniList.krajIzvajanja2 = vpi.krajIzvajanja2 ?? default(int);
            vpisniList.izbirnaSkupina2 = vpi.izbirnaSkupina2 ?? default(int);
            vpisniList.vrstaStudija = vpi.vrstaStudija;
            vpisniList.vrstaVpisa = vpi.vrstaVpisa;
            vpisniList.letnikStudija = vpi.letnikStudija;
            vpisniList.nacinStudija = vpi.nacinStudija;
            vpisniList.oblikaStudija = vpi.oblikaStudija;
            vpisniList.studijskoLetoPrvegaVpisa = vpi.studijskoLetoPrvegaVpisa;
            vpisniList.soglasje1 = vpi.soglasje1 ?? default(bool);
            vpisniList.soglasje2 = vpi.soglasje2 ?? default(bool);

            return vpisniList;
        }

        public static List<profesor> getProfesorsForPredmet(int id)
        {
            studisEntities db = new studisEntities();
            var predmet = db.predmets.SingleOrDefault(v => v.id == id);
            return predmet.profesors.ToList();
        }
    }
}