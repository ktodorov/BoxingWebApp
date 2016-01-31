using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Logins;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using Boxing.Core.Sql.Configurations;
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
            IQueryable<LoginEntity> query = _db.Logins.OrderBy(e => e.Id);

            if (request.Take > 0)
            {
                query = query
                    .Skip(request.Skip)
                    .Take(request.Take);
            }

            var result = (await query
                .ToListAsync()
                .ConfigureAwait(false)
                ).Select(Mapper.Map<LoginDto>);

            return result;
        }
    }
}
