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
    
    public partial class jntuh_ffc_order
    {
        public int id { get; set; }
        public Nullable<int> scheduleID { get; set; }
        public string emails { get; set; }
        public string mobiles { get; set; }
        public string document { get; set; }
        public Nullable<int> isRevisedOrder { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
        public Nullable<System.DateTime> orderDate { get; set; }
        public Nullable<System.DateTime> inspectionDate { get; set; }
    
        public virtual jntuh_ffc_schedule jntuh_ffc_schedule { get; set; }
    }
}
