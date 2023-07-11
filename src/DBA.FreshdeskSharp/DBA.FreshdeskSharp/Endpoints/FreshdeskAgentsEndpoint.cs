using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DBA.FreshdeskSharp.Core;
using DBA.FreshdeskSharp.Models;
using DBA.FreshdeskSharp.Models.Abstractions;
using DBA.FreshdeskSharp.Models.Internal;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Endpoints
{
    public class FreshdeskAgentsEndpoint : FreshdeskEndpointBase
    {
        private readonly string _apiBaseUri;
        private readonly FreshdeskHttpClient _httpClient;
        private readonly JsonSerializerSettings _serializationSettings;

        internal FreshdeskAgentsEndpoint(FreshdeskConfigInternal config, FreshdeskHttpClient httpClient, JsonSerializerSettings serializationSettings)
        {
            _apiBaseUri = config.ApiBaseUri;
            _httpClient = httpClient;
            _serializationSettings = serializationSettings;
        }

        public async Task<FreshdeskAgent> CreateAsync(FreshdeskAgent ticket)
        {
            var requestJson = JsonConvert.SerializeObject(ticket, _serializationSettings);
            var requestUri = $"{_apiBaseUri}/agents";
            using (var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PostAsync(requestUri, requestContent).ConfigureAwait(false))
            {
                return await GetResponseAsync<FreshdeskAgent>(response).ConfigureAwait(false);
            }
        }

        public async Task<FreshdeskAgent> GetAsync(ulong id)
        {
            var requestUri = $"{_apiBaseUri}/agents/{id}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<FreshdeskAgent>(response).ConfigureAwait(false);
            }
        }

        public async Task<List<FreshdeskAgent>> GetListAsync(FreshdeskAgentsListOptions options)
        {
            var query = GetListQueryString(options);
            var requestUri = $"{_apiBaseUri}/agents{query}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<List<FreshdeskAgent>>(response).ConfigureAwait(false);
            }
        }

        private static string GetListQueryString(FreshdeskAgentsListOptions options)
        {
            if (options == null)
            {
                return "";
            }
            var filters = new List<string>();//}
            if (options.Email != default(string))
            {
                filters.Add($"email={WebUtility.UrlEncode(options.Email)}");
            }
            if (options.Email != default(string))
            {
                filters.Add($"mobile={WebUtility.UrlEncode(options.Mobile)}");
            }
            if (options.Email != default(string))
            {
                filters.Add($"phone={WebUtility.UrlEncode(options.Phone)}");
            }
            if (options.State != default(string))
            {
                filters.Add($"state={WebUtility.UrlEncode(options.State)}");
            }
            AddPageFilters(options, filters);
            return $"?{string.Join("&", filters)}";
        }
    }
}