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
    
    public partial class my_aspnet_sessions
    {
        public string SessionId { get; set; }
        public int ApplicationId { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Expires { get; set; }
        public System.DateTime LockDate { get; set; }
        public int LockId { get; set; }
        public int Timeout { get; set; }
        public bool Locked { get; set; }
        public byte[] SessionItems { get; set; }
        public int Flags { get; set; }
    
        public virtual my_aspnet_applications my_aspnet_applications { get; set; }
    }
}
