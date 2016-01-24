using Boxing.Contracts.Requests.Predictions;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Predictions
{
    public class DeletePredictionHandler : CommandHandler<DeletePredictionRequest>
    {
        private readonly BoxingContext _db;

        public DeletePredictionHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(DeletePredictionRequest command)
        {
            var prediction = await _db.Predictions.FindAsync(command.Id).ConfigureAwait(false);

            if (prediction == null)
                throw new NotFoundException();

            _db.Predictions.Remove(prediction);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
