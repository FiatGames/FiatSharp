using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using API.Controllers;
using FiatSharp;
using FiatSharp.Examples.TicTacToe;
using FiatSharp.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes(new FiatSharp.RouteProviders.WebApiConfig.CustomDirectRouteProvider());
            ConverterHelper.AddConverters<Settings, State, Move>(GlobalConfiguration.Configuration.Formatters
                .JsonFormatter.SerializerSettings.Converters);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters
                .Add(new MoveConverter());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters
                .Add(new StringEnumConverter());
        }
    }
}
