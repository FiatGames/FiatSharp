using System;
using System.Linq;
using System.Web.Http;

namespace API
{
    public static partial class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            
        config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "FiatGame", id = RouteParameter.Optional }
            );
        }
    }
}
