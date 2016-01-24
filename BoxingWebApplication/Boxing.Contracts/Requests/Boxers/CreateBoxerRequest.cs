using Boxing.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Requests.Boxers
{
    public class CreateBoxerRequest : IRequest
    {
        public BoxerDto Boxer { get; set; }
    }
}
