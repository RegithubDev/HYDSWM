using COMMON;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEMOSWMCKC
{
    public class RouteValueTransformer : DynamicRouteValueTransformer
    {

       

        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            //string menu = httpContext.User.GetMenuList();
            //List<RoleMaster> mList = JsonConvert.DeserializeObject<List<RoleMaster>>(menu);
            //var Scontroller = values["controller"] as string;
            //var Ccontroller = values["Controller"] as string;
            //var id = await this._productLocator.FindProduct("product", out var controller);
            // RoleMaster info =await Task.Run(() => mList.FirstOrDefault());

            //RouteValueDictionary _Dict = new RouteValueDictionary();
         
            //_Dict.Add("controller", "Asset");
            //_Dict.Add("action", "Index");

            return values;
        }
    }
}
