using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Boxing.Api.Handlers
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "FinishMatch",
            //    routeTemplate: "api/matches/finish/id"
            //);

            config.Routes.MapHttpRoute(
                name: "IsAuthenticatedApi",
                routeTemplate: "api/logins/isauthenticated"
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
