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
    
    public partial class jntuh_scmproceedingsrequests_copy
    {
        public int ID { get; set; }
        public Nullable<int> SCMOrderNew { get; set; }
        public Nullable<int> SCmRequestId { get; set; }
        public Nullable<int> OldSCMId { get; set; }
        public int CollegeId { get; set; }
        public int TotalNoofFacultyRequired { get; set; }
        public int SpecializationId { get; set; }
        public int DEpartmentId { get; set; }
        public int DegreeId { get; set; }
        public Nullable<int> ProfessorsCount { get; set; }
        public Nullable<int> AssociateProfessorsCount { get; set; }
        public Nullable<int> AssistantProfessorsCount { get; set; }
        public string Remarks { get; set; }
        public string SCMNotification { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBY { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public Nullable<bool> ISActive { get; set; }
        public Nullable<System.DateTime> Notificationdate { get; set; }
        public Nullable<int> RequiredProfessor { get; set; }
        public Nullable<int> RequiredAssociateProfessor { get; set; }
        public Nullable<int> RequiredAssistantProfessor { get; set; }
        public int SCMOrder { get; set; }
        public Nullable<System.DateTime> RequestSubmittedDate { get; set; }
    }
}
