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
    
    public partial class jntuh_appeal_principal_registered
    {
        public int id { get; set; }
        public int collegeId { get; set; }
        public string RegistrationNumber { get; set; }
        public Nullable<int> existingFacultyId { get; set; }
        public string IdentifiedFor { get; set; }
        public Nullable<int> DegreeId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> SpecializationId { get; set; }
        public string NOtificationReport { get; set; }
        public string AppointMentOrder { get; set; }
        public string JoiningOrder { get; set; }
        public string SelectionCommiteMinutes { get; set; }
        public string PhysicalPresenceonInspection { get; set; }
        public string AppealReverificationScreenshot { get; set; }
        public string PHDUndertakingDocument { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    }
}
