using Boxing.Contracts;
using BoxingWebApp.Extensions;
using BoxingWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BoxingWebApp.Services
{
    public class WebClientService : IWebClientService
    {
        IConfigurationService configurationService;

        public WebClientService(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public TResponse ExecuteGet<TResponse>(ApiRequest request)
        {
            try
            {
                using (HttpClient client = GetHttpClient())
                {
                    using (var response = client.GetAsync(request.EndPoint).GetAwaiter().GetResult())
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return JsonConvert.DeserializeObject<TResponse>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        }

                        return default(TResponse);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch
            {
                return default(TResponse);
            }
        }

        public TResponse ExecutePost<TResponse>(ApiRequest request)
        {
            try
            {
                using (HttpClient client = GetHttpClient())
                {
                    using (StringContent requestContent = new StringContent(request.Request != null ? JsonConvert.SerializeObject(request.Request) : string.Empty, Encoding.UTF8, "application/json"))
                    {
                        using (var response = client.PostAsync(request.EndPoint, requestContent).GetAwaiter().GetResult())
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                return JsonConvert.DeserializeObject<TResponse>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                            }
                            else if (response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request");
                            }

                            return default(TResponse);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (HttpException ex)
            {
                throw ex;
            }
            catch
            {
                return default(TResponse);
            }
        }

        public TResponse ExecutePut<TResponse>(ApiRequest request)
        {
            try
            {
                using (HttpClient client = GetHttpClient())
                {
                    using (StringContent requestContent = new StringContent(request.Request != null ? JsonConvert.SerializeObject(request.Request) : string.Empty, Encoding.UTF8, "application/json"))
                    {
                        using (var response = client.PutAsync(request.EndPoint, requestContent).GetAwaiter().GetResult())
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                return JsonConvert.DeserializeObject<TResponse>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                            }

                            return default(TResponse);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch
            {
                return default(TResponse);
            }
        }

        public TResponse ExecuteLoginPost<TResponse>(ApiRequest request)
        {
            try
            {
                using (HttpClient client = GetHttpClient("logintoken"))
                {
                    using (StringContent requestContent = new StringContent(request.Request != null ? JsonConvert.SerializeObject(request.Request) : string.Empty, Encoding.UTF8, "application/json"))
                    {
                        using (var response = client.PostAsync(request.EndPoint, requestContent).GetAwaiter().GetResult())
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                return JsonConvert.DeserializeObject<TResponse>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                            }

                            return default(TResponse);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch
            {
                return default(TResponse);
            }
        }

        public void ExecuteDelete(ApiRequest request)
        {
            try
            {
                using (HttpClient client = GetHttpClient())
                {
                    using (var response = client.DeleteAsync(request.EndPoint).GetAwaiter().GetResult())
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch
            {
                return;
            }
        }

        private HttpClient GetHttpClient(string authToken = null)
        {
            HttpClientHandler handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip;
            }

            HttpClient client = new HttpClient(handler);

            client.BaseAddress = new Uri(configurationService.GetValue("ApiBaseUrl"));

            client.Timeout = TimeSpan.FromSeconds(20);

            if (!AuthorizeExtensions.AuthTokenAvailable() &&
                !AuthorizeExtensions.AdminTokenAvailable() &&
                authToken == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (AuthorizeExtensions.AdminTokenAvailable())
            {
                var adminToken = AuthorizeExtensions.GetAdminToken();
                client.DefaultRequestHeaders.Add(Constants.Headers.AdminTokenHeader, adminToken);
            }
            if (authToken == null && AuthorizeExtensions.AuthTokenAvailable())
            {
                authToken = AuthorizeExtensions.GetAuthToken();
            }

            if (authToken != null)
            {
                client.DefaultRequestHeaders.Add(Constants.Headers.AuthTokenHeader, authToken);
            }

            if (handler.SupportsAutomaticDecompression)
            {
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            }

            return client;
        }
    }
}
