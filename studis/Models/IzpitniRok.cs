using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace studis.Models
{
    [Table("IzpitniRok")]
    public class IzpitniRok
    {
        public int ID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum")]
        public DateTime datum { get; set; }

        [Display(Name = "Predmet")]
        public predmet predmet { get; set; }

        [Display(Name = "Profesor")]
        public virtual ICollection<profesor> profesors { get; set; }

        public virtual ICollection<student> students { get; set; }

    }
}