using IMS.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;


namespace IMS.Filters{
    public class ITAuthorizationFilter : IAuthorizationFilter{
        public void OnAuthorization(AuthorizationFilterContext context){
            var session = context.HttpContext.Session;
            bool? perms = session.GetInt32("ITPerms") == 1;

            Endpoint? endpoint = context.HttpContext.GetEndpoint();
            
            //Pages that need IT perms need to have the [RequireITPerms] attribute
            if(endpoint?.Metadata?.GetMetadata<RequireITPerms>() == null){
                return;
            }

            string? prevPage = context.HttpContext.Request.Headers["Referer"].ToString();
             
            //if user doesn't have IT permissions, redirect to the previous page, or login if the referer field is blank
            if (perms == null || !perms.Value){
                if (prevPage.IsNullOrEmpty()){
                    prevPage = "InventoryManagerLanding";
                }
                session.SetString("ITRedirectError", "An IT account is needed to access that page.");
                context.Result = new RedirectResult(prevPage);
            }
        }
   
    }
}