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
    
    public partial class jntuh_college_batch_performance
    {
        public int id { get; set; }
        public string collegeCode { get; set; }
        public string batch { get; set; }
        public int passedAcademicYearId { get; set; }
        public Nullable<int> enrolledStudents { get; set; }
        public Nullable<int> passedStudents { get; set; }
    
        public virtual jntuh_academic_year jntuh_academic_year { get; set; }
    }
}