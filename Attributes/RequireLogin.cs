using System;

//In case we want to specify a login requirement explicitly per page rather than implicitly
//used in conjunction with the LoginAuthorizationFilter

//not necessary for implicit login requirement

namespace IMS.Attributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequireLogin : Attribute{
        public RequireLogin(){}
    }
}