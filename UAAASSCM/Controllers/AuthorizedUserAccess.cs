using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UAAASSCM.Models;
namespace UAAASSCM.Controllers
{
    public class AuthorizedUserAccess : AuthorizeAttribute
    {
        private SCMEntities db = new SCMEntities();
        private readonly string[] allowedroles;
        public AuthorizedUserAccess(params string[] AccessLevel)  
       {
           this.allowedroles = AccessLevel;  
       }  
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;  
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            foreach (var role in allowedroles)
            {
                var user = GetUserRights(httpContext.User.Identity.Name.ToString(), role); // checking active users with allowed roles.  
                if (user > 0)
                {
                    authorize= true; /* return true if Entity has current user(active) with specific role */
                }
            }


            return authorize;
          
        }

        private int GetUserRights(string UserName,string RoleName)
        {
            int roleName = 0;
            if (!string.IsNullOrEmpty(UserName))
            {
                roleName =db.my_aspnet_roles.Join(db.jntuh_registration, R => R.id, U => U.RoleType,(R, U) => new {R = R, U = U}).Where(e =>e.U.Email == UserName && (e.R.id == 1 || e.R.id == 7 || e.R.id == 8) &&e.R.name == RoleName).Select(e => e.R.name).Count();
            }
            return roleName;

        }
    }
}