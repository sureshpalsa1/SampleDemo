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
    
    public partial class jntuh_reinspection_college_laboratories
    {
        public int id { get; set; }
        public int CollegeID { get; set; }
        public int EquipmentID { get; set; }
        public string EquipmentName { get; set; }
        public Nullable<int> EquipmentNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string EquipmentUniqueID { get; set; }
        public Nullable<int> AvailableUnits { get; set; }
        public Nullable<decimal> AvailableArea { get; set; }
        public string RoomNumber { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
        public Nullable<System.DateTime> EquipmentDateOfPurchasing { get; set; }
        public string EquipmentPhoto { get; set; }
        public Nullable<System.DateTime> DelivaryChalanaDate { get; set; }
        public string LabName { get; set; }
        public Nullable<bool> NOPIDOC { get; set; }
        public Nullable<bool> NODCdoc { get; set; }
    
        public virtual jntuh_college jntuh_college { get; set; }
        public virtual jntuh_lab_master jntuh_lab_master { get; set; }
    }
}
