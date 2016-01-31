using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Matches;
using Boxing.Contracts.Requests.Users;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Threading.Tasks;
using System.Linq;

namespace Boxing.Core.Handlers.Features.Users
{
    public class GetUserHandler : IRequestHandler<GetUserRequest, UserDto>
    {
        private readonly BoxingContext _db;

        public GetUserHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<UserDto> HandleAsync(GetUserRequest request)
        {
            var boxer = _db.Users.FirstOrDefault(u => u.Username == request.Username);

            if (boxer == null)
                throw new NotFoundException();

            return Mapper.Map<UserDto>(boxer);
        }
    }
}
