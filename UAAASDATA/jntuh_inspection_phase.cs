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
    
    public partial class jntuh_inspection_phase
    {
        public jntuh_inspection_phase()
        {
            this.college_grade = new HashSet<college_grade>();
            this.college_undertaking = new HashSet<college_undertaking>();
        }
    
        public int id { get; set; }
        public string inspectionPhase { get; set; }
        public int academicYearId { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    
        public virtual ICollection<college_grade> college_grade { get; set; }
        public virtual ICollection<college_undertaking> college_undertaking { get; set; }
        public virtual jntuh_academic_year jntuh_academic_year { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
    }
}
