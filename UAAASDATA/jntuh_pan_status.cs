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
    
    public partial class jntuh_pan_status
    {
        public int Id { get; set; }
        public string PANNumber { get; set; }
        public string PANStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public int createdby { get; set; }
        public System.DateTime CreateOn { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> Updatedby { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}
