using Boxing.Contracts;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Count;
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
    public class CountController : ApiController
    {
        private readonly IMediator _mediator;

        public CountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<int> Get([FromUri] string model)
        {
            var request = new GetCountRequest
            {
                Model = model
            };
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }
    }
}
