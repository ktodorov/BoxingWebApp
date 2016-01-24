using Autofac;
using Boxing.Contracts;
using Boxing.Core.Handlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers
{
    public class Mediator : IMediator
    {
        private readonly ILifetimeScope _scope;

        public Mediator(ILifetimeScope scope)
        {
            _scope = scope;
        }

        [DebuggerStepThrough]
        public Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            dynamic handler = _scope.Resolve(handlerType);

            return handler.HandleAsync((dynamic)request);
        }
    }
}
