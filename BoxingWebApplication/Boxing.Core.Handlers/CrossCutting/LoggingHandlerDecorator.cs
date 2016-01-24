using Boxing.Contracts;
using Boxing.Core.Handlers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.CrossCutting
{
    public class LoggingHandlerDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : class, IRequest<TResponse>, new()
    {
        //private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IRequestHandler<TRequest, TResponse> _decorated;

        public LoggingHandlerDecorator(IRequestHandler<TRequest, TResponse> decorated)
        {
            _decorated = decorated;
        }

        [DebuggerStepThrough]
        public async Task<TResponse> HandleAsync(TRequest request)
        {
            TResponse response;

            //Log.Info("Executing request: " + JsonConvert.SerializeObject(request));

            try
            {
                response = await _decorated.HandleAsync(request);
            }
            catch (Exception e)
            {
                //Log.Error("Exception: " + e.Message);
                throw;
            }

            //Log.Info("Returning response: " + JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
