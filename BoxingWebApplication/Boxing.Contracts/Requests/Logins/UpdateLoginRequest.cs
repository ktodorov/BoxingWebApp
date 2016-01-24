using Boxing.Contracts.Dto;

namespace Boxing.Contracts.Requests.Logins
{
    public class UpdateLoginRequest : IRequest
    {
        public LoginDto Login { get; set; }
    }
}
