//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieWebSite.DataService
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblProducer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblProducer()
        {
            this.tblMovies = new HashSet<tblMovy>();
        }
    
        public decimal ProducerId { get; set; }
        public byte GenderId { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string ProducerName { get; set; }
        public string Bio { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTimeOffset CreatedDateTime { get; set; }
        public System.DateTimeOffset LastModfiedDateTime { get; set; }
    
        public virtual tblGender tblGender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMovy> tblMovies { get; set; }
    }
}