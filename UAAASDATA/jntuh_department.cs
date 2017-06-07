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
    
    public partial class jntuh_department
    {
        public jntuh_department()
        {
            this.jntuh_college_faculty = new HashSet<jntuh_college_faculty>();
            this.jntuh_college_pgcourses = new HashSet<jntuh_college_pgcourses>();
            this.jntuh_college_principal_director = new HashSet<jntuh_college_principal_director>();
            this.jntuh_ffc_auditor = new HashSet<jntuh_ffc_auditor>();
            this.jntuh_lab_master_experments = new HashSet<jntuh_lab_master_experments>();
            this.jntuh_lab_master = new HashSet<jntuh_lab_master>();
            this.jntuh_registered_faculty = new HashSet<jntuh_registered_faculty>();
            this.jntuh_registered_faculty_oldfaculty = new HashSet<jntuh_registered_faculty_oldfaculty>();
            this.jntuh_reinspection_registered_faculty = new HashSet<jntuh_reinspection_registered_faculty>();
            this.jntuh_specialization = new HashSet<jntuh_specialization>();
        }
    
        public int id { get; set; }
        public string departmentName { get; set; }
        public int degreeId { get; set; }
        public string departmentDescription { get; set; }
        public Nullable<bool> CircuitType { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    
        public virtual ICollection<jntuh_college_faculty> jntuh_college_faculty { get; set; }
        public virtual ICollection<jntuh_college_pgcourses> jntuh_college_pgcourses { get; set; }
        public virtual ICollection<jntuh_college_principal_director> jntuh_college_principal_director { get; set; }
        public virtual jntuh_degree jntuh_degree { get; set; }
        public virtual ICollection<jntuh_ffc_auditor> jntuh_ffc_auditor { get; set; }
        public virtual ICollection<jntuh_lab_master_experments> jntuh_lab_master_experments { get; set; }
        public virtual ICollection<jntuh_lab_master> jntuh_lab_master { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
        public virtual ICollection<jntuh_registered_faculty> jntuh_registered_faculty { get; set; }
        public virtual ICollection<jntuh_registered_faculty_oldfaculty> jntuh_registered_faculty_oldfaculty { get; set; }
        public virtual ICollection<jntuh_reinspection_registered_faculty> jntuh_reinspection_registered_faculty { get; set; }
        public virtual ICollection<jntuh_specialization> jntuh_specialization { get; set; }
    }
}
