using Boxing.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Interfaces
{
    public interface IMediator
    {
        Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request);
    }
}
