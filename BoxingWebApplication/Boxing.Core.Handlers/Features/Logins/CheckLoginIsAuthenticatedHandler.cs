using AutoMapper;
using Boxing.Contracts.Extensions;
using Boxing.Contracts.Requests.Logins;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Linq;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Logins
{
    public class CheckLoginIsAuthenticatedHandler : IRequestHandler<CheckLoginIsAuthenticatedRequest, bool>
    {
        private readonly BoxingContext _db;

        public CheckLoginIsAuthenticatedHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<bool> HandleAsync(CheckLoginIsAuthenticatedRequest request)
        {
            var login = _db.Logins.FirstOrDefault(l => l.AuthToken == request.AuthToken);

            if (login == null)
                throw new NotFoundException();

            return CommonExtensions.IsAuthenticated(request.AuthToken, request.AdminRights);
        }
    }
}
