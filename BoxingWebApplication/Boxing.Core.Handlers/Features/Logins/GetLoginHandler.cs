using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Logins;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Logins
{
    public class GetLoginHandler : IRequestHandler<GetLoginRequest, LoginDto>
    {
        private readonly BoxingContext _db;

        public GetLoginHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<LoginDto> HandleAsync(GetLoginRequest request)
        {
            var boxer = await _db.Logins.FindAsync(request.Id).ConfigureAwait(false);

            if (boxer == null)
                throw new NotFoundException();

            return Mapper.Map<LoginDto>(boxer);
        }
    }
}
