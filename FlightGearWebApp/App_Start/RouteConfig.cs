using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");




            routes.MapRoute(
            name: "display",
            url: "display/{ip}/{port}/{time}",
            defaults: new { controller = "Home", action = "display" });

            routes.MapRoute(
            name: "load",
            url: "load/{file}/{time}",
            defaults: new { controller = "Home", action = "load" });

            routes.MapRoute(
            name: "save",
            url: "save/{ip}/{port}/{time}/{period}/{fileName}",
            defaults: new { controller = "Home", action = "save" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
