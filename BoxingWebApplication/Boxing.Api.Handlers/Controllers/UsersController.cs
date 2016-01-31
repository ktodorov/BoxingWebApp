using Boxing.Contracts;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Users;
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
    public class UsersController : ApiController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> Get([FromUri] GetAllUsersRequest request)
        {
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("api/users/details/{username}")]
        public async Task<UserDto> Details(string username)
        {
            var request = new GetUserRequest
            {
                Username = username
            };
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpPut]
        public async Task Update(int id, [FromBody] UserDto userAccount)
        {
            userAccount.Id = id;
            var request = new UpdateUserRequest
            {
                User = userAccount
            };
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task Delete([FromUri] DeleteUserRequest request)
        {
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<LoginDto> Register([FromBody] UserDto user)
        {
            var request = new RegisterUserRequest
            {
                User = user
            };
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }
    }
}
