using Boxing.Core.Sql;
using System.Linq;

namespace Boxing.Contracts.Extensions
{
    public static class CommonExtensions
    {
        public static bool IsValidAdminToken(string token)
        {
            using (var context = new BoxingContext())
            {
                var login = context.Logins.Where(l => l.AuthToken == token).FirstOrDefault();

                if (login != null && login.ExpirationDate.IsValidExpirationDate())
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == login.UserId);
                    if (user != null && user.IsAdmin)
                    {
                        login.ExpirationDate = DateTimeExtensions.CreateExpirationDate();
                        context.SaveChangesAsync();
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsValidToken(string token)
        {
            if (token == "logintoken" || token == "registertoken")
            {
                return true;
            }

            using (var context = new BoxingContext())
            {
                var login = context.Logins.Where(l => l.AuthToken == token).FirstOrDefault();

                if (login != null && login.ExpirationDate.IsValidExpirationDate())
                {
                    login.ExpirationDate = DateTimeExtensions.CreateExpirationDate();
                    context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public static bool IsAuthenticated(string token, bool adminRights)
        {
            var rights = IsValidAdminToken(token);

            if (!adminRights && rights == false)
            {
                rights = rights || IsValidToken(token);
            }

            return rights;
        }
    }
}
