using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Helpers.Users;
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
            var dbSet = _db.Users;
            var query = dbSet.OrderBy(e => e.Id);

            switch (request.Sort)
            {
                case UserSortingOptions.FullNameAscending:
                    query = dbSet.OrderBy(e => e.FullName);
                    break;
                case UserSortingOptions.FullNameDescending:
                    query = dbSet.OrderByDescending(e => e.FullName);
                    break;
                case UserSortingOptions.RatingAscending:
                    query = dbSet.OrderBy(e => e.Rating);
                    break;
                case UserSortingOptions.RatingDescending:
                    query = dbSet.OrderByDescending(e => e.Rating);
                    break;
            }

            return (await query
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync()
                .ConfigureAwait(false)).Select(Mapper.Map<UserDto>);
        }
    }
}
