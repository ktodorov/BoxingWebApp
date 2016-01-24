using BoxingWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxingWebApp.Services
{
    public interface IWebClientService
    {
        TResponse ExecuteGet<TResponse>(ApiRequest request);

        TResponse ExecutePost<TResponse>(ApiRequest request);

        TResponse ExecutePut<TResponse>(ApiRequest request);

        TResponse ExecuteLoginPost<TResponse>(ApiRequest request);

        void ExecuteDelete(ApiRequest request);
    }
}
