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
    
    public partial class my_aspnet_usersinroles
    {
        public int userId { get; set; }
        public int roleId { get; set; }
        public int id { get; set; }
    
        public virtual my_aspnet_roles my_aspnet_roles { get; set; }
        public virtual my_aspnet_users my_aspnet_users { get; set; }
    }
}
