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
    
    public partial class jntuh_college_screens_assigned
    {
        public int Id { get; set; }
        public int CollegeId { get; set; }
        public int ScreenId { get; set; }
        public bool IsEditable { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual jntuh_college jntuh_college { get; set; }
        public virtual jntuh_college_screens jntuh_college_screens { get; set; }
    }
}
