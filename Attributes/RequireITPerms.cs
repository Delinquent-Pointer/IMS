using System;

//attribute to mark that a page requires IT permissions
//used in conjunction with the ITAuthorizationFilter
namespace IMS.Attributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequireITPerms : Attribute{
        public RequireITPerms(){}
    }
}