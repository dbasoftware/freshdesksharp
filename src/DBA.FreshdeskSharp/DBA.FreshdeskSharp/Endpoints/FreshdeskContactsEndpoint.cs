using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DBA.FreshdeskSharp.Core;
using DBA.FreshdeskSharp.Models;
using DBA.FreshdeskSharp.Models.Internal;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Endpoints
{
    public class FreshdeskContactsEndpoint : FreshdeskEndpointBase
    {
        private readonly string _apiBaseUri;
        private readonly FreshdeskHttpClient _httpClient;
        private readonly JsonSerializerSettings _serializationSettings;

        internal FreshdeskContactsEndpoint(FreshdeskConfigInternal config, FreshdeskHttpClient httpClient, JsonSerializerSettings serializationSettings)
        {
            _apiBaseUri = config.ApiBaseUri;
            _httpClient = httpClient;
            _serializationSettings = serializationSettings;
        }

        public async Task<List<FreshdeskContact<FreshdeskCustomFields>>> GetListAsync(FreshdeskContactListOptions options = null)
        {
            return await GetListAsync<FreshdeskCustomFields>(options).ConfigureAwait(false);
        }

        public async Task<List<FreshdeskContact<TCustomFieldObject>>> GetListAsync<TCustomFieldObject>(FreshdeskContactListOptions options = null) where TCustomFieldObject : class
        {
            var query = GetListQueryString(options);
            var requestUri = $"{_apiBaseUri}/contacts{query}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                var result = await GetResponseAsync<List<FreshdeskContactInternal<TCustomFieldObject>>>(response).ConfigureAwait(false);
                return result.Select(contact => contact.ToContact()).ToList();
            }
        }

        public async Task<FreshdeskContact<FreshdeskCustomFields>> GetAsync(ulong id)
        {
            return await GetAsync<FreshdeskCustomFields>(id).ConfigureAwait(false);
        }

        public async Task<FreshdeskContact<TCustomFieldObject>> GetAsync<TCustomFieldObject>(ulong id) where TCustomFieldObject : class
        {
            var requestUri = $"{_apiBaseUri}/contacts/{id}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                var internalContact = await GetResponseAsync<FreshdeskContactInternal<TCustomFieldObject>>(response).ConfigureAwait(false);
                return internalContact.ToContact();
            }
        }

        public async Task<FreshdeskContact<TCustomFieldObject>> CreateAsync<TCustomFieldObject>(FreshdeskContact<TCustomFieldObject> contact) where TCustomFieldObject : class
        {
            var requestJson = JsonConvert.SerializeObject(FreshdeskContactInternal<TCustomFieldObject>.FromContact(contact), _serializationSettings);
            var requestUri = $"{_apiBaseUri}/contacts";
            using (var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PostAsync(requestUri, requestContent).ConfigureAwait(false))
            {
                return (await GetResponseAsync<FreshdeskContactInternal<TCustomFieldObject>>(response).ConfigureAwait(false)).ToContact();
            }
        }

        public async Task<FreshdeskContact<TCustomFieldObject>> UpdateAsync<TCustomFieldObject>(FreshdeskContact<TCustomFieldObject> contact) where TCustomFieldObject : class
        {
            var requestJson = JsonConvert.SerializeObject(FreshdeskContactInternal<TCustomFieldObject>.FromContact(contact), _serializationSettings);
            var requestUri = $"{_apiBaseUri}/contacts/{contact.Id}";
            using (var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PutAsync(requestUri, requestContent).ConfigureAwait(false))
            {
                return (await GetResponseAsync<FreshdeskContactInternal<TCustomFieldObject>>(response).ConfigureAwait(false)).ToContact();
            }
        }

        public async Task RestoreAsync(ulong id)
        {
            var requestUri = $"{_apiBaseUri}/contacts/{id}/restore";
            using (var requestContent = new StringContent("", Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PutAsync(requestUri, requestContent).ConfigureAwait(false))
            {
                await GetResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(ulong id)
        {
            var requestUri = $"{_apiBaseUri}/contacts/{id}";
            using (var response = await _httpClient.DeleteAsync(requestUri).ConfigureAwait(false))
            {
                await GetResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<List<FreshdeskContactField>> GetFieldsAsync()
        {
            var requestUri = $"{_apiBaseUri}/contact_fields";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                var result = await GetResponseAsync<List<FreshdeskContactField>>(response).ConfigureAwait(true);
                return result;
            }
        }

        private static string GetListQueryString(FreshdeskContactListOptions options)
        {
            if (options == null)
            {
                return "";
            }
            var filters = new List<string>();
            if (options.Email != default(string))
            {
                filters.Add($"email={WebUtility.UrlEncode(options.Email)}");
            }
            if (options.Phone != default(ulong?))
            {
                filters.Add($"phone={options.Phone.Value}");
            }
            if (options.Mobile != default(ulong?))
            {
                filters.Add($"mobile={options.Mobile.Value}");
            }
            if (options.CompanyId != default(ulong?))
            {
                filters.Add($"company_id={options.CompanyId.Value}");
            }
            if (options.State != default(FreshdeskContactState?))
            {
                filters.Add($"state={options.State.ToString().ToLower()}");
            }
            AddPageFilters(options, filters);
            return filters.Count == 0 ? "" : $"?{string.Join("&", filters)}";
        }
    }
}