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
    
    public partial class jntuh_college_lab
    {
        public int id { get; set; }
        public int collegeId { get; set; }
        public int specializationId { get; set; }
        public int shiftId { get; set; }
        public int yearInDegreeId { get; set; }
        public string labName { get; set; }
        public int totalExperiments { get; set; }
        public decimal labFloorArea { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
        public Nullable<bool> isShared { get; set; }
    
        public virtual jntuh_college jntuh_college { get; set; }
        public virtual jntuh_specialization jntuh_specialization { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
        public virtual jntuh_shift jntuh_shift { get; set; }
        public virtual jntuh_year_in_degree jntuh_year_in_degree { get; set; }
    }
}
