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
    
    public partial class jntuh_college_affiliation
    {
        public int id { get; set; }
        public int collegeId { get; set; }
        public int affiliationTypeId { get; set; }
        public Nullable<System.DateTime> affiliationFromDate { get; set; }
        public Nullable<System.DateTime> affiliationToDate { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
        public string affiliationGrade { get; set; }
        public Nullable<int> affiliationDuration { get; set; }
        public string affiliationStatus { get; set; }
        public string CGPA { get; set; }
    
        public virtual jntuh_affiliation_type jntuh_affiliation_type { get; set; }
        public virtual jntuh_college jntuh_college { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
    }
}
