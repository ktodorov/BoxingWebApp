using Boxing.Contracts.Dto;
using System.Collections.Generic;

namespace Boxing.Contracts.Requests.Logins
{
    public class GetAllLoginsRequest : IRequest<IEnumerable<LoginDto>>
    {
        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
