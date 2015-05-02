using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace studis.Models
{
    public class IzpitniRokModel
    {
        public int id { get; set; }

        [Remote("PreveriDatum", "IzpitniRok", HttpMethod = "POST", ErrorMessage = "Datum ne sme biti v preteklosti.")]
        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}")]//, ApplyFormatInEditMode = true)]
        [Display(Name = "Datum")]
        public DateTime datum { get; set; }

        [Display(Name = "Predmet")]
        public int predmet { get; set; }

        [Display(Name = "Profesor")]
        public int profesor { get; set; }

    }
}