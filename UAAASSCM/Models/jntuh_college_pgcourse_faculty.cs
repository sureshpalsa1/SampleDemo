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
    
    public partial class jntuh_college_pgcourse_faculty
    {
        public int id { get; set; }
        public Nullable<int> courseId { get; set; }
        public string facultyName { get; set; }
        public string designation { get; set; }
        public string UG { get; set; }
        public string PG { get; set; }
        public string Phd { get; set; }
        public string UGSpecialization { get; set; }
        public string PGSpecialization { get; set; }
        public string PhdSpecialization { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
    }
}
