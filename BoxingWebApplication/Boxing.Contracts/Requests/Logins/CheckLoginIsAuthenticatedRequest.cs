using Boxing.Contracts.Dto;

namespace Boxing.Contracts.Requests.Logins
{
    public class CheckLoginIsAuthenticatedRequest : IRequest<bool>
    {
        public int Id { get; set; }
        public string AuthToken { get; set; }
        
        public bool AdminRights { get; set; }
    }
}
