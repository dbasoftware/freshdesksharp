using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DBA.FreshdeskSharp.Models.Abstractions;

namespace DBA.FreshdeskSharp.Core
{
    internal class FreshdeskHttpClient : IDisposable
    {
        private readonly IFreshdeskClientLogger _logger;
        private readonly bool _retryWhenThrottled;
        private readonly HttpClient _httpClient;
        private readonly TimeSpan? _timeout;
        private const int HttpStatusTooManyRequests = 429;

        public FreshdeskHttpClient(FreshdeskConfig config)
        {
            _logger = config.Logger;
            _retryWhenThrottled = config.RetryWhenThrottled;
            _timeout = config.Timeout;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {config.Credentials.GetEncodedCredentials()}");
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        private async Task<HttpResponseMessage> RequestAsync(Func<TimeSpan?, Task<HttpResponseMessage>> request)
        {
            var completeBy = _timeout == null 
                ? (DateTime?)null 
                : DateTime.Now + _timeout;
            var zeroTimeSpan = new TimeSpan(0, 0, 0);
            while (true)
            {
                var timeout = completeBy == null 
                    ? null 
                    : completeBy - DateTime.Now;
                var result = await request(timeout).ConfigureAwait(false);
                if ((int)result.StatusCode == HttpStatusTooManyRequests && _retryWhenThrottled)
                {
                    var retryAfter = result.Headers.RetryAfter;
                    if (retryAfter.Delta == null)
                    {
                        throw new Exception("Request throttled without returning Retry-After header");
                    }
                    timeout = completeBy == null
                        ? null
                        : completeBy - DateTime.Now;
                    var retryTimeSpan = retryAfter.Delta.Value;
                    if (timeout != null && (timeout < zeroTimeSpan || timeout < retryTimeSpan))
                    {
                        return result;
                    }
                    result.Dispose();
                    await Task.Delay(retryAfter.Delta.Value);
                }
                else
                {
                    return result;
                }
            }
        }
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            _logger?.Log($"GET request: {requestUri}");
            return await RequestAsync(async timeout =>
                {
                    if (timeout == null)
                    {
                        return await _httpClient.GetAsync(requestUri).ConfigureAwait(false);
                    }
                    var cts = new CancellationTokenSource((TimeSpan)timeout);
                    return await _httpClient.GetAsync(requestUri, cts.Token).ConfigureAwait(false);
                })
                .ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, StringContent requestContent)
        {
            _logger?.Log($"POST request: {requestUri}");
            _logger?.Log($"POST content:\r\n{await requestContent.ReadAsStringAsync()}");
            return await RequestAsync(async timeout =>
                {
                    if (timeout == null)
                    {
                        return await _httpClient.PostAsync(requestUri, requestContent).ConfigureAwait(false);
                    }
                    var cts = new CancellationTokenSource((TimeSpan)timeout);
                    return await _httpClient.PostAsync(requestUri, requestContent, cts.Token).ConfigureAwait(false);
                })
                .ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, StringContent requestContent)
        {
            _logger?.Log($"PUT request: {requestUri}");
            _logger?.Log($"PUT content:\r\n{await requestContent.ReadAsStringAsync()}");
            return await RequestAsync(async timeout =>
                {
                    if (timeout == null)
                    {
                        return await _httpClient.PutAsync(requestUri, requestContent).ConfigureAwait(false);
                    }
                    var cts = new CancellationTokenSource((TimeSpan)timeout);
                    return await _httpClient.PutAsync(requestUri, requestContent, cts.Token).ConfigureAwait(false);
                })
                .ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            _logger?.Log($"DELETE request: {requestUri}");
            return await RequestAsync(async timeout =>
                {
                    if (timeout == null)
                    {
                        return await _httpClient.DeleteAsync(requestUri).ConfigureAwait(false);
                    }
                    var cts = new CancellationTokenSource((TimeSpan)timeout);
                    return await _httpClient.DeleteAsync(requestUri, cts.Token).ConfigureAwait(false);
                })
                .ConfigureAwait(false);
        }
    }
}