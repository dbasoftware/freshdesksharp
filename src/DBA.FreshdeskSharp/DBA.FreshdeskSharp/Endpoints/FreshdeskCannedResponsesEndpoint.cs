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
    public class FreshdeskCannedResponsesEndpoint : FreshdeskEndpointBase
    {
        private readonly string _apiBaseUri;
        private readonly FreshdeskHttpClient _httpClient;
        private readonly JsonSerializerSettings _serializationSettings;

        internal FreshdeskCannedResponsesEndpoint(FreshdeskConfigInternal config, FreshdeskHttpClient httpClient, JsonSerializerSettings serializationSettings)
        {
            _apiBaseUri = config.ApiBaseUri;
            _httpClient = httpClient;
            _serializationSettings = serializationSettings;
        }

        public async Task<List<FreshdeskCannedResponse>> GetListAsync(ulong cannedResponseFolderId) 
        {
            var requestUri = $"{_apiBaseUri}/canned_response_folders/{cannedResponseFolderId}/responses";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<List<FreshdeskCannedResponse>>(response).ConfigureAwait(false);
            }
        }

        public async Task<FreshdeskCannedResponse> GetAsync(ulong cannedResponseId)
        {
            var requestUri = $"{_apiBaseUri}/canned_responses/{cannedResponseId}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<FreshdeskCannedResponse>(response).ConfigureAwait(false);
            }
        }

    }
}