using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Boxers;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Boxers
{
    public class GetBoxerHandler : IRequestHandler<GetBoxerRequest, BoxerDto>
    {
        private readonly BoxingContext _db;

        public GetBoxerHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<BoxerDto> HandleAsync(GetBoxerRequest request)
        {
            var boxer = await _db.Boxers.FindAsync(request.Id).ConfigureAwait(false);

            if (boxer == null)
                throw new NotFoundException();

            return Mapper.Map<BoxerDto>(boxer);
        }
    }
}
