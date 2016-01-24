using Boxing.Contracts;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Logins;
using Boxing.Core.Handlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Boxing.Api.Handlers.Controllers
{
    public class LoginsController : ApiController
    {
        private readonly IMediator _mediator;

        public LoginsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<LoginDto>> Get([FromUri] GetAllLoginsRequest request)
        {
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<LoginDto> Get([FromUri] int id)
        {
            var request = new GetLoginRequest
            {
                Id = id
            };
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpPut]
        public async Task Update(int id, [FromBody] LoginDto login)
        {
            login.Id = id;
            var request = new UpdateLoginRequest
            {
                Login = login
            };
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task Delete([FromUri] DeleteLoginRequest request)
        {
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<LoginDto> Login([FromBody] UserDto user)
        {
            var request = new LogUserLoginRequest
            {
                User = user
            };
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("api/logins/isauthenticated")]
        public async Task<bool> IsAuthenticated([FromBody] string token, [FromUri] bool adminRights = false)
        {
            var request = new CheckLoginIsAuthenticatedRequest
            {
                AuthToken = token,
                AdminRights = adminRights
            };

            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }
    }
}
