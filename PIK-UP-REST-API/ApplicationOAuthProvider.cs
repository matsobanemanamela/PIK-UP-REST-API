using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using PIK_UP_REST_API.Models;
using System.Data.Entity;

namespace PIK_UP_REST_API
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var db = new PIKUPEntities();

            var user = db.UserTables.FirstOrDefault(x => x.Email == context.UserName && x.Password == context.Password);
           // var administrator = db.Administrators.FirstOrDefault(x => x.UserName == context.UserName && x.Password == context.Password);

            if (user != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("UserID", user.UserID.ToString()));
                identity.AddClaim(new Claim("Name", user.Name));
                identity.AddClaim(new Claim("Surname", user.Surname));
                identity.AddClaim(new Claim("StudentNumber", user.StudentNumber.ToString()));
                identity.AddClaim(new Claim("Email", user.Email));
                identity.AddClaim(new Claim("Institution", user.Institution));
                identity.AddClaim(new Claim("MobileNumber", user.MobileNumber));
                identity.AddClaim(new Claim("Password", user.Password));
                identity.AddClaim(new Claim("Image", user.Image.ToString()));
                identity.AddClaim(new Claim("Gender", user.Gender));
                context.Validated(identity);
            }
          /*  else if (administrator != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("AdministratorID", administrator.AdministratorID.ToString()));
                identity.AddClaim(new Claim("Firstname", administrator.Firstname));
                identity.AddClaim(new Claim("Lastname", administrator.Lastname));
                identity.AddClaim(new Claim("UserName", administrator.UserName));
                identity.AddClaim(new Claim("Password", administrator.Password));
                context.Validated(identity);

            }*/
            else
            {
                return;
            }
        }
    }
}