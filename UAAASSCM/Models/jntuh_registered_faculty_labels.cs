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
    
    public partial class jntuh_registered_faculty_labels
    {
        public jntuh_registered_faculty_labels()
        {
            this.jntuh_college_faculty_verified = new HashSet<jntuh_college_faculty_verified>();
        }
    
        public int Id { get; set; }
        public string LabelName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual ICollection<jntuh_college_faculty_verified> jntuh_college_faculty_verified { get; set; }
    }
}
