using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace studis.Models
{
    public class VpisniList
    {
        [Display(Name = "Vpisna številka")]
        public int vpisnaStevilka { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Ime")]
        public string ime { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Priimek")]
        public string priimek { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Datum rojstva")]
        public DateTime datumRojstva { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Kraj rojstva")]
        public string krajRojstva { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Občina rojstva")]
        public string obcinaRojstva { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Država rojstva")]
        public string drzavaRojstva { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Državljanstvo")]
        public string drzavljanstvo { get; set; }

        [Required]
        [Display(Name = "Spol")]
        public char spol { get; set; }

        [Required]
        [Display(Name = "Emšo")]
        public long emso { get; set; }

        [Required]
        [Display(Name = "Davčna številka")]
        public int davcnaStevilka { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email naslov")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Mobilni telefon")]
        public string prenosniTelefon { get; set; }

        //prebivalisce
        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Naslov")]
        public string naslov { get; set; }

        [Display(Name = "Naslov za vročanje?")]
        public bool vrocanje { get; set; }

        [Required]
        [Display(Name = "Poštna številka")]
        public int postnaStevilka { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Občina")]
        public string obcina { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Država")]
        public string drzava { get; set; }

        //zacasni naslov
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Naslov")]
        public string naslovZacasni { get; set; }

        [Display(Name = "Naslov za vročanje?")]
        public bool vrocanjeZacasni { get; set; }

        [Display(Name = "Poštna številka")]
        public int postnaStevilkaZacasni { get; set; }

        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Občina")]
        public string obcinaZacasni { get; set; }

        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Država")]
        public string drzavaZacasni { get; set; }

        //studij
        [Required]
        [Display(Name = "Študijski program")]
        public int studijskiProgram { get; set; }

        [Required]
        [Display(Name = "Smer")]
        public int smer { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Kraj izvajanja")]
        public string krajIzvajanja { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Izbirna skupina")]
        public string izbirnaSkupina { get; set; }

        [Display(Name = "Drugi študijski program")]
        public int studijskiProgram2 { get; set; }

        [Display(Name = "Druga smer")]
        public int smer2 { get; set; }

        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Drug kraj izvajanja")]
        public string krajIzvajanja2 { get; set; }

        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Druga izbirna skupina")]
        public string izbirnaSkupina2 { get; set; }

        [Required]
        [Display(Name = "Vrsta študija")]
        public int vrstaStudija { get; set; }

        [Required]
        [Display(Name = "Vrsta vpisa")]
        public int vrstaVpisa { get; set; }

        [Required]
        [Display(Name = "Letnik študija")]
        public int letnikStudija { get; set; }

        [Required]
        [Display(Name = "Način študija")]
        public int nacinStudija { get; set; }

        [Required]
        [Display(Name = "Oblika študija")]
        public int oblikaStudija { get; set; }

        [StringLength(100, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Študijsko leto prvega vpisa")]
        public string studijskoLetoPrvegaVpisa { get; set; }

        [Required]
        [Display(Name = "Soglasje")]
        public bool soglasje1 { get; set; }

        [Required]
        [Display(Name = "Soglasje")]
        public bool soglasje2 { get; set; }

    }
}