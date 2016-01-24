using Boxing.Contracts.Requests.Matches;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Matches
{
    public class DeleteMatchHandler : CommandHandler<DeleteMatchRequest>
    {
        private readonly BoxingContext _db;

        public DeleteMatchHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(DeleteMatchRequest command)
        {
            var match = await _db.Matches.FindAsync(command.Id).ConfigureAwait(false);

            if (match == null)
                throw new NotFoundException();

            _db.Matches.Remove(match);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
