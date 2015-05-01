using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace studis.Models
{
    public class IzpitniRokModel
    {
        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Datum")]
        public DateTime datum { get; set; }

        [Display(Name = "Predmet")]
        public int predmet { get; set; }

        [Display(Name = "Profesor")]
        public int profesor { get; set; }

    }
}