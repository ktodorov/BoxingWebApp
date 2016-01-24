using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Requests.Predictions
{
    public class DeletePredictionRequest : IRequest
    {
        public int Id { get; set; }
    }
}
