using IMS.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IMS.Filters{
    public class LoginAuthorizationFilter : IAuthorizationFilter{
        public void OnAuthorization(AuthorizationFilterContext context){
            var session = context.HttpContext.Session;
            string? user = session.GetString("User");

            Endpoint? endpoint = context.HttpContext.GetEndpoint();

            //this makes a login requirement implicit:
            //all pages that do not have the [AllowAnonymous] attribute will require login
            if(endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null){
                return;
            }

            //explicit requirement for login:
            //all pages that do have the [RequireLogin] attribute will require login
            // if(endpoint?.Metadata?.GetMetadata<RequireLogin>() == null){
            //     return;
            // }
            
            //redirects to the login page if the user session attribute doesnt exist (user is not logged in)
            if (string.IsNullOrEmpty(user)){
                session.SetString("LoginRedirectError", "Please log in to access that page.");
                context.Result = new RedirectResult("Login");
            }
        }  
    }
}