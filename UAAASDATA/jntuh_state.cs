//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UAAASDATA
{
    using System;
    using System.Collections.Generic;
    
    public partial class jntuh_state
    {
        public jntuh_state()
        {
            this.jntuh_address = new HashSet<jntuh_address>();
            this.jntuh_affiliation_requests = new HashSet<jntuh_affiliation_requests>();
            this.jntuh_district = new HashSet<jntuh_district>();
        }
    
        public int id { get; set; }
        public string stateName { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    
        public virtual ICollection<jntuh_address> jntuh_address { get; set; }
        public virtual ICollection<jntuh_affiliation_requests> jntuh_affiliation_requests { get; set; }
        public virtual ICollection<jntuh_district> jntuh_district { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
    }
}
