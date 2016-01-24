using Boxing.Contracts.Dto;
using Boxing.Core.Handlers.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Boxing.Contracts.Requests.Matches;
using System.Net.Http;
using System.Net;

namespace Boxing.Api.Handlers.Controllers
{
    public class MatchesController : ApiController
    {
        private readonly IMediator _mediator;

        public MatchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<MatchDto>> Get([FromUri] GetAllMatchesRequest request)
        {
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<MatchDto> Get([FromUri] int id)
        {
            var request = new GetMatchRequest
            {
                Id = id
            };
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody] MatchDto userAccount)
        {
            var request = new CreateMatchRequest
            {
                Match = userAccount
            };
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task Update(int id, [FromBody] MatchDto match)
        {
            match.Id = id;
            var request = new UpdateMatchRequest
            {
                Match = match
            };
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpPut]
        [Route("api/matches/{id}/finish/")]
        public async Task Finish(int id, [FromBody] int winnerId)
        {
            var request = new FinishMatchRequest
            {
                Id = id,
                WinnerId = winnerId
            };
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task Delete([FromUri] DeleteMatchRequest request)
        {
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }
    }
}
