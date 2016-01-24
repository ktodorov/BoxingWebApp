using AutoMapper;
using Boxing.Contracts.Requests.Matches;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Linq;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Matches
{
    public class FinishMatchHandler : CommandHandler<FinishMatchRequest>
    {
        private readonly BoxingContext _db;

        public FinishMatchHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(FinishMatchRequest command)
        {
            var affectedPredictions = _db.Predictions.Where(p => p.MatchId == command.Id).Select(p => p.UserId).ToList();
            var affectedUsers = _db.Users.ToList().Where(u => affectedPredictions.Contains(u.Id)).ToList();

            var userIds = affectedUsers.Select(u => u.Id).ToList();

            var userPredictions = _db.Predictions.ToList().Where(p => userIds.Contains(p.UserId)).ToList();
            var userPredictionMatchIds = userPredictions.Select(p => p.MatchId).ToList();

            var predictedMatches = _db.Matches
                                      .Where(m => userPredictionMatchIds.Contains(m.Id) && m.WinnerId != null)
                                      .ToList();

            var matchEntity = await _db.Matches.FindAsync(command.Id).ConfigureAwait(false);

            if (matchEntity == null)
                throw new NotFoundException();

            matchEntity.WinnerId = command.WinnerId;

            foreach (var user in affectedUsers)
            {
                var successfull = 0.0;
                var currentUserPredictions = userPredictions.Where(p => p.UserId == user.Id).ToList();
                foreach (var prediction in currentUserPredictions)
                {
                    var match = predictedMatches.FirstOrDefault(m => m.Id == prediction.MatchId);

                    if (match != null)
                    {
                        // Ако е новия завършил мач не трябва да гледаме записа в базата
                        if ((match.Id == matchEntity.Id &&
                            matchEntity.WinnerId == prediction.PredictedBoxerId) ||
                            (match.Id != matchEntity.Id &&
                             match.WinnerId == prediction.PredictedBoxerId))
                        {
                            successfull++;
                        }
                    }
                }

                user.Rating = successfull / (double)currentUserPredictions.Count;
            }

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
