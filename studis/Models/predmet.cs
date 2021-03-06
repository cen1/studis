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
    
    public partial class predmet
    {
        public predmet()
        {
            this.izvajanjes = new HashSet<izvajanje>();
        }
    
        public long id { get; set; }
        public string ime { get; set; }
        public string opis { get; set; }
        public int kreditne { get; set; }
        public int semester { get; set; }
        public string koda { get; set; }
        public int letnik { get; set; }
        public bool obvezen { get; set; }
        public Nullable<long> modulId { get; set; }
        public Nullable<bool> prostoizbirni { get; set; }
        public Nullable<bool> strokovnoizbirni { get; set; }
        public int studijskiProgram { get; set; }
        public int vrstaStudija { get; set; }
    
        public virtual modul modul { get; set; }
        public virtual sifrant_letnik sifrant_letnik { get; set; }
        public virtual sifrant_studijskiprogram sifrant_studijskiprogram { get; set; }
        public virtual sifrant_klasius sifrant_klasius { get; set; }
        public virtual ICollection<izvajanje> izvajanjes { get; set; }
    }
}
