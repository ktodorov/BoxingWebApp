using Boxing.Contracts;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Extensions;
using BoxingWebApp.Services;
using BoxingWebApp.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BoxingWebApp.Extensions
{
    public static class AuthorizeExtensions
    {
        public static bool AuthTokenAvailable()
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session[Constants.Headers.AuthTokenHeader] == null)
            {
                return false;
            }

            return true;
        }

        public static bool AdminTokenAvailable()
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session[Constants.Headers.AdminTokenHeader] == null)
            {
                return false;
            }

            return true;
        }

        public static string GetAuthToken()
        {
            return HttpContext.Current.Session[Constants.Headers.AuthTokenHeader].ToString();
        }

        public static string GetAdminToken()
        {
            return HttpContext.Current.Session[Constants.Headers.AdminTokenHeader].ToString();
        }

        public static bool IsAuthenticated()
        {
            if (!AuthTokenAvailable() && !AdminTokenAvailable())
            {
                return false;
            }

            var authToken = string.Empty;
            if (AdminTokenAvailable())
            {
                authToken = GetAdminToken();
            }
            else
            {
                authToken = GetAuthToken();
            }

            var webClient = new WebClientService(new ConfigurationService());
            var isAuthenticated = webClient.ExecutePost<bool>(new Models.ApiRequest() { EndPoint = "logins/isauthenticated", Request = authToken });

            return isAuthenticated;
        }

        public static bool CurrentUserIsAdmin()
        {
            if (!AdminTokenAvailable())
            {
                return false;
            }

            var adminToken = GetAdminToken();

            var webClient = new WebClientService(new ConfigurationService());
            var hasAdminRights = webClient.ExecutePost<bool>(new Models.ApiRequest()
            {
                EndPoint = "logins/isauthenticated?adminRights=true",
                Request = adminToken
            });

            return hasAdminRights;
        }

        public static UserDto GetCurrentUser()
        {
            var authToken = string.Empty;
            if (AdminTokenAvailable())
            {
                authToken = GetAdminToken();
            }
            else if (AuthTokenAvailable())
            {
                authToken = GetAuthToken();
            }
            else
            {
                return null;
            }

            var webClient = new WebClientService(new ConfigurationService());
            var logins = webClient.ExecuteGet<IEnumerable<LoginDto>>(new Models.ApiRequest() { EndPoint = "logins?skip=0&take=100" })
                ?.Select(q => new LoginsListItem() { Id = q.Id, AuthToken = q.AuthToken, ExpirationDate = q.ExpirationDate, UserId = q.UserId })?.ToList();

            var currentLogin = logins?.FirstOrDefault(l => l.AuthToken == authToken);

            if (currentLogin == null)
            {
                return null;
            }

            var users = webClient.ExecuteGet<IEnumerable<UserDto>>(new Models.ApiRequest() { EndPoint = "users?skip=0&take=100" })?.ToList();

            var currentUser = users.FirstOrDefault(u => u.Id == currentLogin.UserId);

            return currentUser;
        }
    }
}
