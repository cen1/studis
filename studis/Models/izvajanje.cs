//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace studis.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class izvajanje
    {
        public izvajanje()
        {
            this.izpitniroks = new HashSet<izpitnirok>();
        }
    
        public long id { get; set; }
        public long predmetId { get; set; }
        public long izvajalec1Id { get; set; }
        public Nullable<long> izvajalec2Id { get; set; }
        public Nullable<long> izvajalec3Id { get; set; }
        public int studijskoletoId { get; set; }
    
        public virtual profesor profesor { get; set; }
        public virtual profesor profesor1 { get; set; }
        public virtual profesor profesor2 { get; set; }
        public virtual predmet predmet { get; set; }
        public virtual ICollection<izpitnirok> izpitniroks { get; set; }
        public virtual sifrant_studijskoleto sifrant_studijskoleto { get; set; }
    }
}
