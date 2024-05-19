using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMMON;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace HYDSWM
{
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {

        public string Permissions { get; set; } //Permission string to get from controller

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string menu = context.HttpContext.User.GetMenuList();
            if (!string.IsNullOrEmpty(menu))
            {
                List<RoleMaster> mList = JsonConvert.DeserializeObject<List<RoleMaster>>(menu);
                if (mList != null)
                {

                    var descriptor = context?.ActionDescriptor as ControllerActionDescriptor;
                    if (descriptor != null)
                    {
                        var actionName = descriptor.ActionName.ToLower();
                        var ctrlName = descriptor.ControllerName.ToLower();
                        string queryString = context.HttpContext.Request.QueryString.ToString().ToLower();
                        string Joinstr = actionName + queryString;
                       
                        //if(Joinstr.ToLower()=="AllAsset?TId=0".ToLower() || Joinstr.ToLower() == "AllEmployee?TId=0".ToLower() || Joinstr.ToLower() == "AllSlaMatrix?cid=0".ToLower())
                        //{
                        //    actionName = actionName + queryString;
                        //}
                        
                        //if (actionName == "AllAsset?TId=0" || actionName == "AllEmployee?TId=0"||actionName== "AllSlaMatrix?cid=0")
                        //    actionName = actionName + queryString;
                        bool isView = mList.Any(x => x.an.Split('?')[0].ToLower() == actionName && x.cn.ToLower() == ctrlName);
                        if (isView)
                            return;
                        else
                        {
                            //context.Result = new UnauthorizedResult();
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "AccessDenied" }));
                            return;
                        }

                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
    public class CustomPostAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {

        public string Permissions { get; set; } //Permission string to get from controller

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string menu = context.HttpContext.User.GetUserId();
            if (!string.IsNullOrEmpty(menu))
            {
                return;
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
