using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DBA.FreshdeskSharp.Core;
using DBA.FreshdeskSharp.Models;
using DBA.FreshdeskSharp.Models.Internal;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Endpoints
{
    public class FreshdeskCompaniesEndpoint : FreshdeskEndpointBase
    {
        private readonly string _apiBaseUri;
        private readonly FreshdeskHttpClient _httpClient;
        private readonly JsonSerializerSettings _serializationSettings;

        internal FreshdeskCompaniesEndpoint(FreshdeskConfigInternal config, FreshdeskHttpClient httpClient, JsonSerializerSettings serializationSettings)
        {
            _apiBaseUri = config.ApiBaseUri;
            _httpClient = httpClient;
            _serializationSettings = serializationSettings;
        }

        public async Task<List<FreshdeskCompany<FreshdeskCustomFields>>> GetListAsync(FreshdeskCompaniesListOptions options = null)
        {
            return await GetListAsync<FreshdeskCustomFields>(options).ConfigureAwait(false);
        }

        public async Task<List<FreshdeskCompany<TCustomFieldObject>>> GetListAsync<TCustomFieldObject>(FreshdeskCompaniesListOptions options = null) where TCustomFieldObject : class
        {
            var query = GetListQueryString(options);
            var requestUri = $"{_apiBaseUri}/companies{query}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<List<FreshdeskCompany<TCustomFieldObject>>>(response).ConfigureAwait(false);
            }
        }

        public async Task<FreshdeskCompany<FreshdeskCustomFields>> GetAsync(ulong id)
        {
            return await GetAsync<FreshdeskCustomFields>(id).ConfigureAwait(false);
        }

        public async Task<FreshdeskCompany<TCustomFieldObject>> GetAsync<TCustomFieldObject>(ulong id) where TCustomFieldObject : class
        {
            var requestUri = $"{_apiBaseUri}/companies/{id}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<FreshdeskCompany<TCustomFieldObject>>(response).ConfigureAwait(false);
            }
        }

        public async Task<FreshdeskCompany<TCustomFieldObject>> CreateAsync<TCustomFieldObject>(FreshdeskCompany<TCustomFieldObject> company) where TCustomFieldObject : class
        {
            var requestJson = JsonConvert.SerializeObject(company, _serializationSettings);
            var requestUri = $"{_apiBaseUri}/companies";
            using (var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PostAsync(requestUri, requestContent).ConfigureAwait(false))
            {
                return await GetResponseAsync<FreshdeskCompany<TCustomFieldObject>>(response).ConfigureAwait(false);
            }
        }

        public async Task<FreshdeskCompany<TCustomFieldObject>> UpdateAsync<TCustomFieldObject>(FreshdeskCompany<TCustomFieldObject> company) where TCustomFieldObject : class
        {
            var requestJson = JsonConvert.SerializeObject(company, _serializationSettings);
            var requestUri = $"{_apiBaseUri}/companies/{company.Id}";
            using (var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PutAsync(requestUri, requestContent).ConfigureAwait(false))
            {
                return await GetResponseAsync<FreshdeskCompany<TCustomFieldObject>>(response).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(ulong id)
        {
            var requestUri = $"{_apiBaseUri}/companies/{id}";
            using (var response = await _httpClient.DeleteAsync(requestUri).ConfigureAwait(false))
            {
                await GetResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<List<FreshdeskCompanyField>> GetFieldsAsync()
        {
            var requestUri = $"{_apiBaseUri}/company_fields";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<List<FreshdeskCompanyField>>(response).ConfigureAwait(true);
            }
        }

        private static string GetListQueryString(FreshdeskListOptionsBase options)
        {
            if (options == null)
            {
                return "";
            }
            var filters = new List<string>();
            AddPageFilters(options, filters);
            return filters.Count == 0 ? "" : $"?{string.Join("&", filters)}";
        }
    }
}
