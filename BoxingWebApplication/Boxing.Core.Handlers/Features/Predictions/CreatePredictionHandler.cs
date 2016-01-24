using AutoMapper;
using Boxing.Contracts.Requests.Predictions;
using Boxing.Core.Sql;
using Boxing.Core.Sql.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Predictions
{
    public class CreatePredictionHandler : CommandHandler<CreatePredictionRequest>
    {
        private readonly BoxingContext _db;

        public CreatePredictionHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(CreatePredictionRequest command)
        {
            var entity = Mapper.Map<PredictionEntity>(command.Prediction);
            _db.Predictions.Add(entity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
