using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vidly
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // coustom route, the order of routes need to be defined from most spasific to most generic
            routes.MapRoute(
                "MoviesByReleaseDate",
                // the URL path 
                "movie/releaseDate/{year}/{month}",
                // the controller and action that will execute
                // if either of these change in code they need to change here which can be annoying
                new { controller = "Movie", action = "ByReleaseDate" },
                // year has 4 digets and month has 2
                new { year = @"\d{4}" , month = @"\d{2}"});
                //new { year = @"2015|2016", month = @"\d{4}" })

            // Attribute route is better to use then the convetional way .MapRoute
            // becouse if the action changes we dont need to change it here
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
