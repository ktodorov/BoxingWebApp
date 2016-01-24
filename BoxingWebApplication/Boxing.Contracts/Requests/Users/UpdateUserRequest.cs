using Boxing.Contracts.Dto;

namespace Boxing.Contracts.Requests.Users
{
    public class UpdateUserRequest : IRequest
    {
        public UserDto User { get; set; }
    }
}
