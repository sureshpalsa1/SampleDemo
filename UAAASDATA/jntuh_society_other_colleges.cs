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
    
    public partial class jntuh_society_other_colleges
    {
        public int id { get; set; }
        public int collegeId { get; set; }
        public string collegeName { get; set; }
        public int affiliatedUniversityId { get; set; }
        public string otherUniversityName { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
        public Nullable<int> yearOfEstablishment { get; set; }
    
        public virtual jntuh_college jntuh_college { get; set; }
        public virtual jntuh_university jntuh_university { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
    }
}
