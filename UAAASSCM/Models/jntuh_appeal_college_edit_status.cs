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
    
    public partial class jntuh_appeal_college_edit_status
    {
        public int id { get; set; }
        public int collegeId { get; set; }
        public bool IsCollegeEditable { get; set; }
        public Nullable<System.DateTime> editFromDate { get; set; }
        public Nullable<System.DateTime> editToDate { get; set; }
        public string DeclarationPath { get; set; }
        public string FurtherAppealSupportingDocument { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    }
}
