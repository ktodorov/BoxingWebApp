using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Logins;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Logins
{
    public class GetAllLoginsHandler : IRequestHandler<GetAllLoginsRequest, IEnumerable<LoginDto>>
    {
        private readonly BoxingContext _db;

        public GetAllLoginsHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<LoginDto>> HandleAsync(GetAllLoginsRequest request)
        {
            return (await _db.Logins
                .OrderBy(e => e.Id)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync()
                .ConfigureAwait(false)
                ).Select(Mapper.Map<LoginDto>);
        }
    }
}
