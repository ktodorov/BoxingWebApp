using AutoMapper;
using Boxing.Contracts.Requests.Matches;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Matches
{
    public class UpdateMatchHandler : CommandHandler<UpdateMatchRequest>
    {
        private readonly BoxingContext _db;

        public UpdateMatchHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(UpdateMatchRequest command)
        {
            var match = await _db.Matches.FindAsync(command.Match.Id).ConfigureAwait(false);

            if (match == null)
                throw new NotFoundException();

            Mapper.Map(command.Match, match);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
