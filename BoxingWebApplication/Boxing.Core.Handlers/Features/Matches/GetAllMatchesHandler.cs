using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Matches;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Matches
{
    public class GetAllMatchesHandler : IRequestHandler<GetAllMatchesRequest, IEnumerable<MatchDto>>
    {
        private readonly BoxingContext _db;

        public GetAllMatchesHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<MatchDto>> HandleAsync(GetAllMatchesRequest request)
        {
            var dbSet = _db.Matches
                .Include(e => e.Boxer1)
                .Include(e => e.Boxer2)
                .Include(e => e.Winner);

            if (!string.IsNullOrEmpty(request.Search))
            {
                dbSet = dbSet.Where(m => m.Boxer1.Name.Contains(request.Search) ||
                                         m.Boxer2.Name.Contains(request.Search) ||
                                         m.Address.Contains(request.Search));
            }

            if (request.PastUnfinished.HasValue && request.PastUnfinished.Value)
            {
                var currentTime = DateTime.Now;
                dbSet = dbSet.Where(m => m.Time < currentTime &&
                                         m.WinnerId == null);
            }

            var result = (await query
                .ToListAsync()
                .ConfigureAwait(false)).Select(Mapper.Map<MatchDto>);

            return result;
        }
    }
}
