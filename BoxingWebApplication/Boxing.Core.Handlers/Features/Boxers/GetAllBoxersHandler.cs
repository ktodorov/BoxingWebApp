using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Boxers;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Boxers
{
    public class GetAllBoxersHandler : IRequestHandler<GetAllBoxersRequest, IEnumerable<BoxerDto>>
    {
        private readonly BoxingContext _db;

        public GetAllBoxersHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<BoxerDto>> HandleAsync(GetAllBoxersRequest request)
        {
            return (await _db.Boxers
                .OrderBy(e => e.Id)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync()
                .ConfigureAwait(false)).Select(Mapper.Map<BoxerDto>);
        }
    }
}
