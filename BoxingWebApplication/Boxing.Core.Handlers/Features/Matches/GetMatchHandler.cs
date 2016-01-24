using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Matches;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Boxing.Core.Handlers.Features.Matches
{
    public class GetMatchHandler : IRequestHandler<GetMatchRequest, MatchDto>
    {
        private readonly BoxingContext _db;

        public GetMatchHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<MatchDto> HandleAsync(GetMatchRequest request)
        {
            var match = await _db.
                                Matches.
                                Include(e => e.Boxer1).
                                Include(e => e.Boxer2).
                                Include(e => e.Winner).
                                SingleOrDefaultAsync(e => e.Id == request.Id).
                                ConfigureAwait(false);

            if (match == null)
                throw new NotFoundException();

            return Mapper.Map<MatchDto>(match);
        }
    }
}
