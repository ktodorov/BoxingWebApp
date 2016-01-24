using Boxing.Contracts;
using Boxing.Contracts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Boxing.Api.Handlers.Filters
{
    public class AuthAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            IEnumerable<string> values;

            var token = string.Empty;
            if (headers.TryGetValues(Constants.Headers.AdminTokenHeader, out values))
            {
                token = values.FirstOrDefault();

                if (!CommonExtensions.IsValidAdminToken(token))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else if (headers.TryGetValues(Constants.Headers.AuthTokenHeader, out values))
            {
                token = values.FirstOrDefault();

                if (!CommonExtensions.IsValidToken(token))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            base.OnAuthorization(actionContext);
        }
    }
}
