using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Boxers;
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
    public class BoxersController : ApiController
    {
        private readonly IMediator _mediator;

        public BoxersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<BoxerDto>> Get([FromUri] GetAllBoxersRequest request)
        {
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<BoxerDto> Get([FromUri] int id)
        {
            var request = new GetBoxerRequest
            {
                Id = id
            };
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody] BoxerDto boxer)
        {
            var request = new CreateBoxerRequest
            {
                Boxer = boxer
            };
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task Update(int id, [FromBody] BoxerDto boxer)
        {
            boxer.Id = id;
            var request = new UpdateBoxerRequest
            {
                Boxer = boxer
            };
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task Delete([FromUri] DeleteBoxerRequest request)
        {
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }
    }
}
