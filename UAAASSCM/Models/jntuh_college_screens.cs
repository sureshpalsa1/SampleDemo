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
    
    public partial class jntuh_college_screens
    {
        public jntuh_college_screens()
        {
            this.jntuh_college_screens_assigned = new HashSet<jntuh_college_screens_assigned>();
            this.jntuh_college_screens_assigned_log = new HashSet<jntuh_college_screens_assigned_log>();
        }
    
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual ICollection<jntuh_college_screens_assigned> jntuh_college_screens_assigned { get; set; }
        public virtual ICollection<jntuh_college_screens_assigned_log> jntuh_college_screens_assigned_log { get; set; }
    }
}
