using AutoMapper;
using Boxing.Contracts.Requests.Count;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Linq;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Count
{
    public class GetCountHandler : IRequestHandler<GetCountRequest, int>
    {
        private readonly BoxingContext _db;

        public GetCountHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<int> HandleAsync(GetCountRequest request)
        {
            switch (request.Model)
            {
                case "boxers":
                    return _db.Boxers.Count();
                case "matches":
                    return _db.Matches.Count();
                case "users":
                    return _db.Users.Count();
                case "logins":
                    return _db.Logins.Count();
            }

            return 0;
        }
    }
}
