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
    
    public partial class jntuh_college_overall_faculty_studentratio
    {
        public int id { get; set; }
        public int academicYearId { get; set; }
        public int collegeId { get; set; }
        public int specializationId { get; set; }
        public int shiftId { get; set; }
        public int sanctionedIntake { get; set; }
        public int totalFaculty { get; set; }
        public int ratifiedFaculty { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    
        public virtual jntuh_academic_year jntuh_academic_year { get; set; }
        public virtual jntuh_college jntuh_college { get; set; }
        public virtual jntuh_specialization jntuh_specialization { get; set; }
        public virtual jntuh_shift jntuh_shift { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
    }
}