using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Users;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Users
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, IEnumerable<UserDto>>
    {
        private readonly BoxingContext _db;

        public GetAllUsersHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserDto>> HandleAsync(GetAllUsersRequest request)
        {
            return (await _db.Users
                .OrderBy(e => e.Id)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync()
                .ConfigureAwait(false)).Select(Mapper.Map<UserDto>);
        }
    }
}
