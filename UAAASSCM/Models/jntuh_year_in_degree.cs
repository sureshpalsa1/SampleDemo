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
    
    public partial class jntuh_year_in_degree
    {
        public jntuh_year_in_degree()
        {
            this.jntuh_college_academic_performance = new HashSet<jntuh_college_academic_performance>();
            this.jntuh_college_fee_reimbursement = new HashSet<jntuh_college_fee_reimbursement>();
            this.jntuh_college_lab = new HashSet<jntuh_college_lab>();
        }
    
        public int id { get; set; }
        public string yearInDegree { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    
        public virtual ICollection<jntuh_college_academic_performance> jntuh_college_academic_performance { get; set; }
        public virtual ICollection<jntuh_college_fee_reimbursement> jntuh_college_fee_reimbursement { get; set; }
        public virtual ICollection<jntuh_college_lab> jntuh_college_lab { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
    }
}
