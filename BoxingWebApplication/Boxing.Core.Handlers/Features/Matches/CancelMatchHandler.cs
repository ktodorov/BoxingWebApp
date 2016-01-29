using AutoMapper;
using Boxing.Contracts.Requests.Matches;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Matches
{
    public class CancelMatchHandler : CommandHandler<CancelMatchRequest>
    {
        private readonly BoxingContext _db;

        public CancelMatchHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(CancelMatchRequest command)
        {
            var predictions = _db.Predictions.Where(p => p.MatchId == command.Id).ToList();

            var match = _db.Matches.Find(command.Id);
            if (match == null)
            {
                throw new NotFoundException();
            }

            match.Time = DateTime.MaxValue;

            _db.Predictions.RemoveRange(predictions);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
