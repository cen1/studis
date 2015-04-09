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

        [Display(Name = "Vpisna številka")]
        public int? vpisnaStevilka { get; set; }

        [StringLength(45, ErrorMessage = "{0} mora biti dolgo vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Študijsko leto")]
        public string studijskoLeto { get; set; }

        [Required]
        [Remote("PreveriIme", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neveljavna oblika imena.")]
        [StringLength(45, ErrorMessage = "{0} mora biti dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Ime")]
        public string ime { get; set; }

        [Required]
        [Remote("PreveriPriimek", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neveljavna oblika priimka.")]
        [StringLength(45, ErrorMessage = "{0} mora bitio dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Priimek")]
        public string priimek { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Remote("PreveriDatum", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neveljaven datum.")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Datum rojstva")]
        public DateTime datumRojstva { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "{0} mora biti dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Kraj rojstva")]
        public string krajRojstva { get; set; }

        [Required]
        [Display(Name = "Občina rojstva")]
        public int obcinaRojstva { get; set; }

        [Required]
        [Display(Name = "Država rojstva")]
        public int drzavaRojstva { get; set; }

        [Required]
        [Display(Name = "Državljanstvo")]
        public int drzavljanstvo { get; set; }

        [Required]
        [Display(Name = "Spol")]
        public int spol { get; set; }

        [Required]
        [Remote("PreveriEmso", "VpisniList", HttpMethod = "POST", ErrorMessage = "Neveljaven EMŠO.")]
        [Display(Name = "EMŠO")]
        public string emso { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "{0} mora biti dolga {2} znakov.", MinimumLength = 8)]
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
        [StringLength(45, ErrorMessage = "{0} mora biti dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Naslov")]
        public string naslov { get; set; }

        [Display(Name = "Naslov za vročanje?")]
        public bool vrocanje { get; set; }

        [Required]
        [Display(Name = "Poštna številka")]
        public int postnaStevilka { get; set; }

        [Required]
        [Display(Name = "Občina")]
        public int obcina { get; set; }

        [Required]
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
        public int? drzavaZacasni { get; set; }

        //studij
        [Required]
        [Display(Name = "Študijski program")]
        public int studijskiProgram { get; set; }

        [Required]
        [Display(Name = "Smer/Usmeritev/Modul/Znastveno področje")]
        public int smer { get; set; }

        [Required]
        [Display(Name = "Kraj izvajanja")]
        public int krajIzvajanja { get; set; }

        [Required]
        [Display(Name = "Izbirna skupina")]
        public int izbirnaSkupina { get; set; }

        [Display(Name = "Drugi študijski program")]
        public int? studijskiProgram2 { get; set; }

        [Display(Name = "Smer/Usmeritev/Modul/Znastveno področje")]
        public int? smer2 { get; set; }

        [Display(Name = "Kraj izvajanja")]
        public int? krajIzvajanja2 { get; set; }

        [Display(Name = "Izbirna skupina")]
        public int? izbirnaSkupina2 { get; set; }

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

        [Display(Name = "Študijsko leto prvega vpisa")]
        public int? studijskoLetoPrvegaVpisa { get; set; }

        [Required]
        [Display(Name = "Soglasje za koriščenje storitev knjižničnega sitema.")]
        public bool soglasje1 { get; set; }

        [Required]
        [Display(Name = "Soglasje za obveščanje o aktualnih študijskih zadevah ter za karierno svetovanje in za druge aktivnosti, povezane z zagotavljanjem kakovosti.")]
        public bool soglasje2 { get; set; }
    }
}