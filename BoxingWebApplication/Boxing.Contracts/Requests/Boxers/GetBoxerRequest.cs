using Boxing.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Requests.Boxers
{
    public class GetBoxerRequest : IRequest<BoxerDto>
    {
        public int Id { get; set; }
    }
}
