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
    
    public partial class jntuh_appeal_mbadeficiency
    {
        public int Id { get; set; }
        public Nullable<int> CollegeId { get; set; }
        public Nullable<int> ComputersDeficencyCount { get; set; }
        public string MacAddressDocument { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public Nullable<System.DateTime> Updatedon { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
