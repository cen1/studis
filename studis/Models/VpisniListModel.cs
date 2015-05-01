using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace studis.Models
{
    public class VpisniListModel
    {
        [StringLength(45, ErrorMessage = "{0} mora biti dolgo vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Študijsko leto")]
        public string studijskoLeto { get; set; }

        [Required(ErrorMessage="Ime je obvezno.")]
        [Remote("PreveriIme", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neveljavna oblika imena.")]
        [StringLength(45, ErrorMessage = "{0} mora biti dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Ime")]
        public string ime { get; set; }

        [Required(ErrorMessage = "Priimek je obvezen.")]
        [Remote("PreveriPriimek", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neveljavna oblika priimka.")]
        [StringLength(45, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Priimek")]
        public string priimek { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum rojstva")]
        public DateTime datumRojstva { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        public int dr_dan { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        public int dr_mesec { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        public int dr_leto { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [StringLength(45, ErrorMessage = "{0} mora biti dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Kraj rojstva")]
        public string krajRojstva { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Občina rojstva")]
        public int obcinaRojstva { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Remote("PreveriDrzavoInObcino", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neujemanje z občino.", AdditionalFields="obcinaRojstva")]
        [Display(Name = "Država rojstva")]
        public int drzavaRojstva { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Državljanstvo")]
        public int drzavljanstvo { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Spol")]
        public int spol { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Remote("PreveriEmso", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neveljaven EMŠO.")]
        [Display(Name = "EMŠO")]
        public string emso { get; set; }

        //[StringLength(8, ErrorMessage = "{0} mora biti dolga {2} znakov.", MinimumLength = 8)]
        [Range(10000000, 99999999, ErrorMessage = "Davčna štvilka mora biti dolga točno 8 znakov")]
        [Display(Name = "Davčna številka")]
        public int davcnaStevilka { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email je nepravilne oblike.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email naslov")]
        public string email { get; set; }

        [Display(Name = "Prenosni telefon")]
        public string prenosniTelefon { get; set; }

        //prebivalisce
        [Required]
        [StringLength(45, ErrorMessage = "{0} mora biti dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Naslov")]
        public string naslov { get; set; }

        [Display(Name = "Naslov za vročanje?")]
        public bool vrocanje { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Poštna številka")]
        public int postnaStevilka { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Občina")]
        public int obcina { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Remote("PreveriDrzavoInObcinoInPostno", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neujemanje z občino/poštno številko.", AdditionalFields = "obcina,postnaStevilka")]
        [Display(Name = "Država")]
        public int drzava { get; set; }

        //zacasni naslov
        [StringLength(45, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Naslov")]
        public string naslovZacasni { get; set; }

        [Display(Name = "Naslov za vročanje?")]
        public bool vrocanjeZacasni { get; set; }

        [Display(Name = "Poštna številka")]
        public int? postnaStevilkaZacasni { get; set; }

        [Display(Name = "Občina")]
        public int? obcinaZacasni { get; set; }

        [Display(Name = "Država")]
        [Remote("PreveriDrzavoInObcinoInPostno2", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neujemanje z občino/poštno številko.", AdditionalFields = "obcinaZacasni,postnaStevilkaZacasni")]
        public int? drzavaZacasni { get; set; }

        //studij
        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Študijski program")]
        public int studijskiProgram { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Smer")]
        public int smer { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Kraj izvajanja")]
        public int krajIzvajanja { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Izbirna skupina")]
        public int izbirnaSkupina { get; set; }

        [Display(Name = "Drugi študijski program")]
        public int? studijskiProgram2 { get; set; }

        [Display(Name = "Smer")]
        public int? smer2 { get; set; }

        [Display(Name = "Kraj izvajanja")]
        public int? krajIzvajanja2 { get; set; }

        [Display(Name = "Izbirna skupina")]
        public int? izbirnaSkupina2 { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Vrsta študija")]
        public int vrstaStudija { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Remote("PreveriVrstaVpisa", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neveljavna izbira")]
        [Display(Name = "Vrsta vpisa")]
        public int vrstaVpisa { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Letnik študija")]
        public int letnikStudija { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Način študija")]
        public int nacinStudija { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Oblika študija")]
        public int oblikaStudija { get; set; }

        [Display(Name = "Študijsko leto prvega vpisa")]
        public int? studijskoLetoPrvegaVpisa { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Soglasje za koriščenje storitev knjižničnega sitema.")]
        public bool soglasje1 { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Soglasje za obveščanje o aktualnih študijskih zadevah ter za karierno svetovanje in za druge aktivnosti, povezane z zagotavljanjem kakovosti.")]
        public bool soglasje2 { get; set; }
    }
}