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
    
    public partial class jntuh_specialization
    {
        public jntuh_specialization()
        {
            this.jntuh_college_academic_performance = new HashSet<jntuh_college_academic_performance>();
            this.jntuh_college_faculty_deficiency = new HashSet<jntuh_college_faculty_deficiency>();
            this.jntuh_college_fee_reimbursement = new HashSet<jntuh_college_fee_reimbursement>();
            this.jntuh_college_ffc_overall_faculty_studentratio = new HashSet<jntuh_college_ffc_overall_faculty_studentratio>();
            this.jntuh_college_intake_existing = new HashSet<jntuh_college_intake_existing>();
            this.jntuh_college_intake_existing_datentry2 = new HashSet<jntuh_college_intake_existing_datentry2>();
            this.jntuh_college_intake_proposed = new HashSet<jntuh_college_intake_proposed>();
            this.jntuh_college_lab = new HashSet<jntuh_college_lab>();
            this.jntuh_college_laboratories_deficiency = new HashSet<jntuh_college_laboratories_deficiency>();
            this.jntuh_college_overall_faculty_studentratio = new HashSet<jntuh_college_overall_faculty_studentratio>();
            this.jntuh_college_pgcourses = new HashSet<jntuh_college_pgcourses>();
            this.jntuh_college_placement = new HashSet<jntuh_college_placement>();
            this.jntuh_college_teaching_faculty_position = new HashSet<jntuh_college_teaching_faculty_position>();
            this.jntuh_faculty_subjects = new HashSet<jntuh_faculty_subjects>();
            this.jntuh_lab_master = new HashSet<jntuh_lab_master>();
            this.jntuh_lab_master_experments = new HashSet<jntuh_lab_master_experments>();
            this.jntuh_reinspection_college_laboratories_deficiency = new HashSet<jntuh_reinspection_college_laboratories_deficiency>();
        }
    
        public int id { get; set; }
        public string specializationName { get; set; }
        public int departmentId { get; set; }
        public string specializationDescription { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    
        public virtual ICollection<jntuh_college_academic_performance> jntuh_college_academic_performance { get; set; }
        public virtual ICollection<jntuh_college_faculty_deficiency> jntuh_college_faculty_deficiency { get; set; }
        public virtual ICollection<jntuh_college_fee_reimbursement> jntuh_college_fee_reimbursement { get; set; }
        public virtual ICollection<jntuh_college_ffc_overall_faculty_studentratio> jntuh_college_ffc_overall_faculty_studentratio { get; set; }
        public virtual ICollection<jntuh_college_intake_existing> jntuh_college_intake_existing { get; set; }
        public virtual ICollection<jntuh_college_intake_existing_datentry2> jntuh_college_intake_existing_datentry2 { get; set; }
        public virtual ICollection<jntuh_college_intake_proposed> jntuh_college_intake_proposed { get; set; }
        public virtual ICollection<jntuh_college_lab> jntuh_college_lab { get; set; }
        public virtual ICollection<jntuh_college_laboratories_deficiency> jntuh_college_laboratories_deficiency { get; set; }
        public virtual ICollection<jntuh_college_overall_faculty_studentratio> jntuh_college_overall_faculty_studentratio { get; set; }
        public virtual ICollection<jntuh_college_pgcourses> jntuh_college_pgcourses { get; set; }
        public virtual ICollection<jntuh_college_placement> jntuh_college_placement { get; set; }
        public virtual ICollection<jntuh_college_teaching_faculty_position> jntuh_college_teaching_faculty_position { get; set; }
        public virtual jntuh_department jntuh_department { get; set; }
        public virtual ICollection<jntuh_faculty_subjects> jntuh_faculty_subjects { get; set; }
        public virtual ICollection<jntuh_lab_master> jntuh_lab_master { get; set; }
        public virtual ICollection<jntuh_lab_master_experments> jntuh_lab_master_experments { get; set; }
        public virtual ICollection<jntuh_reinspection_college_laboratories_deficiency> jntuh_reinspection_college_laboratories_deficiency { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
    }
}
