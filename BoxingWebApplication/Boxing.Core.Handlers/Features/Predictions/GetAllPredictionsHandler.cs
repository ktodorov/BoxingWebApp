using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Predictions;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Predictions
{
    public class GetAllPredictionsHandler : IRequestHandler<GetAllPredictionsRequest, IEnumerable<PredictionDto>>
    {
        private readonly BoxingContext _db;

        public GetAllPredictionsHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PredictionDto>> HandleAsync(GetAllPredictionsRequest request)
        {
            return (await _db.Predictions
                .OrderBy(e => e.Id)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync()
                .ConfigureAwait(false)).Select(Mapper.Map<PredictionDto>);
        }
    }
}
