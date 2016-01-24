using AutoMapper;
using Boxing.Contracts.Requests.Predictions;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Predictions
{
    public class UpdatePredictionHandler : CommandHandler<UpdatePredictionRequest>
    {
        private readonly BoxingContext _db;

        public UpdatePredictionHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(UpdatePredictionRequest command)
        {
            var prediction = await _db.Predictions.FindAsync(command.Prediction.Id).ConfigureAwait(false);

            if (prediction == null)
                throw new NotFoundException();

            Mapper.Map(command.Prediction, prediction);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
