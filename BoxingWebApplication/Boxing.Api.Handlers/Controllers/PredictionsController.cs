using Boxing.Contracts.Dto;
using Boxing.Core.Handlers.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Boxing.Contracts.Requests.Predictions;
using System.Net.Http;
using System.Net;

namespace Boxing.Api.Handlers.Controllers
{
    public class PredictionsController : ApiController
    {
        private readonly IMediator _mediator;

        public PredictionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<PredictionDto>> Get([FromUri] GetAllPredictionsRequest request)
        {
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<PredictionDto> Get([FromUri] int id)
        {
            var request = new GetPredictionRequest
            {
                Id = id
            };
            return await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Create([FromBody] PredictionDto prediction)
        {
            var request = new CreatePredictionRequest
            {
                Prediction = prediction
            };
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task Update(int id, [FromBody] PredictionDto prediction)
        {
            prediction.Id = id;
            var request = new UpdatePredictionRequest
            {
                Prediction = prediction
            };
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task Delete([FromUri] DeletePredictionRequest request)
        {
            await _mediator.ExecuteAsync(request).ConfigureAwait(false);
        }
    }
}
