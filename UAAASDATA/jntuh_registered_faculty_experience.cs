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
    
    public partial class jntuh_registered_faculty_experience
    {
        public int id { get; set; }
        public Nullable<int> facultyId { get; set; }
        public Nullable<int> collegeId { get; set; }
        public Nullable<int> facultyDesignationId { get; set; }
        public Nullable<System.DateTime> facultyDateOfAppointment { get; set; }
        public Nullable<System.DateTime> facultyDateOfResignation { get; set; }
        public string facultyRelievingLetter { get; set; }
        public string facultyJoiningOrder { get; set; }
        public Nullable<decimal> facultySalary { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
    
        public virtual jntuh_college jntuh_college { get; set; }
        public virtual jntuh_designation jntuh_designation { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
        public virtual jntuh_registered_faculty_oldfaculty jntuh_registered_faculty_oldfaculty { get; set; }
    }
}
