using Boxing.Contracts.Dto;
using Boxing.Contracts.Helpers.Users;
using System.Collections.Generic;

namespace Boxing.Contracts.Requests.Users
{
    public class GetAllUsersRequest : IRequest<IEnumerable<UserDto>>
    {
        public int Skip { get; set; }

        public int Take { get; set; }

        public UserSortingOptions Sort { get; set; }
    }
}
