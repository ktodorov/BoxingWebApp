using Boxing.Contracts.Dto;
using System.Collections.Generic;

namespace Boxing.Contracts.Requests.Users
{
    public class GetAllUsersRequest : IRequest<IEnumerable<UserDto>>
    {
        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
