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
    
    public partial class user_browsers
    {
        public int id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string IPAddress { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string MajorVersion { get; set; }
        public string MinorVersion { get; set; }
        public string Platform { get; set; }
        public string IsBeta { get; set; }
        public string IsCrawler { get; set; }
        public string IsAOL { get; set; }
        public string IsWin16 { get; set; }
        public string IsWin32 { get; set; }
        public string SupportsFrames { get; set; }
        public string SupportsTables { get; set; }
        public string SupportsCookies { get; set; }
        public string SupportsVBScript { get; set; }
        public string SupportsJavaScript { get; set; }
        public string SupportsJavaApplets { get; set; }
        public string SupportsActiveXControls { get; set; }
        public string SupportsJavaScriptVersion { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }
    }
}
