//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UAAASSCM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class jntuh_edep_equipment
    {
        public jntuh_edep_equipment()
        {
            this.jntuh_college_examination_branch_edep = new HashSet<jntuh_college_examination_branch_edep>();
        }
    
        public int id { get; set; }
        public string equipmentName { get; set; }
        public string equipmentDescription { get; set; }
        public Nullable<int> normsEDEP { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    
        public virtual ICollection<jntuh_college_examination_branch_edep> jntuh_college_examination_branch_edep { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
    }
}
