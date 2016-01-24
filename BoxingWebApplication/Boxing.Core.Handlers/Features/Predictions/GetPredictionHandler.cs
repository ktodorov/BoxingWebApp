using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Predictions;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Predictions
{
    public class GetPredictionHandler : IRequestHandler<GetPredictionRequest, PredictionDto>
    {
        private readonly BoxingContext _db;

        public GetPredictionHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<PredictionDto> HandleAsync(GetPredictionRequest request)
        {
            var prediction = await _db.Predictions.FindAsync(request.Id).ConfigureAwait(false);

            if (prediction == null)
                throw new NotFoundException();

            return Mapper.Map<PredictionDto>(prediction);
        }
    }
}
