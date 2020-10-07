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
    public class FreshdeskTicketsEndpoint : FreshdeskEndpointBase
    {
        private readonly string _apiBaseUri;
        private readonly FreshdeskHttpClient _httpClient;
        private readonly JsonSerializerSettings _serializationSettings;

        internal FreshdeskTicketsEndpoint(FreshdeskConfigInternal config, FreshdeskHttpClient httpClient, JsonSerializerSettings serializationSettings)
        {
            _apiBaseUri = config.ApiBaseUri;
            _httpClient = httpClient;
            _serializationSettings = serializationSettings;
        }

        public async Task<FreshdeskTicket<TCustomFieldObject>> CreateAsync<TCustomFieldObject>(FreshdeskTicket<TCustomFieldObject> ticket) where TCustomFieldObject : class
        {
            var requestJson = JsonConvert.SerializeObject(ticket, _serializationSettings);
            var requestUri = $"{_apiBaseUri}/tickets";
            using (var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json"))
            using (var response = await _httpClient.PostAsync(requestUri, requestContent).ConfigureAwait(false))
            {
                return await GetResponseAsync<FreshdeskTicket<TCustomFieldObject>>(response).ConfigureAwait(false);
            }
        }

        public async Task<FreshdeskTicket<FreshdeskCustomFields>> GetAsync(ulong id)
        {
            return await GetAsync<FreshdeskCustomFields>(id).ConfigureAwait(false);
        }

        public async Task<FreshdeskTicket<TCustomFieldObject>> GetAsync<TCustomFieldObject>(ulong id) where TCustomFieldObject : class
        {
            var requestUri = $"{_apiBaseUri}/tickets/{id}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<FreshdeskTicket<TCustomFieldObject>>(response).ConfigureAwait(false);
            }
        }

        public async Task<List<FreshdeskTicket<FreshdeskCustomFields>>> GetListAsync(FreshdeskTicketListOptions options)
        {
            return await GetListAsync<FreshdeskCustomFields>(options).ConfigureAwait(false);
        }

        public async Task<List<FreshdeskTicket<TCustomFieldObject>>> GetListAsync<TCustomFieldObject>(FreshdeskTicketListOptions options) where TCustomFieldObject : class
        {
            var query = GetListQueryString(options);
            var requestUri = $"{_apiBaseUri}/tickets{query}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<List<FreshdeskTicket<TCustomFieldObject>>>(response).ConfigureAwait(false);
            }
        }

        public async Task<FreshdeskTicketSearchResults<FreshdeskCustomFields>> SearchAsync(string searchQuery)
        {
            return await SearchAsync<FreshdeskCustomFields>(searchQuery).ConfigureAwait(false);
        }

        public async Task<FreshdeskTicketSearchResults<TCustomFieldObject>> SearchAsync<TCustomFieldObject>(string searchQuery) where TCustomFieldObject : class
        {
            var query = $"?query=\"{WebUtility.UrlEncode(searchQuery)}\"";
            var requestUri = $"{_apiBaseUri}/search/tickets{query}";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                return await GetResponseAsync<FreshdeskTicketSearchResults<TCustomFieldObject>>(response).ConfigureAwait(false);
            }
        }

        public async Task<FreshdeskTicketSearchResults<FreshdeskCustomFields>> SearchAsync(Expression<Func<IFreshdeskTicketQuery, bool>> searchQuery)
        {
            return await SearchAsync<FreshdeskCustomFields>(searchQuery).ConfigureAwait(false);
        }

        public async Task<FreshdeskTicketSearchResults<TCustomFieldObject>> SearchAsync<TCustomFieldObject>(Expression<Func<IFreshdeskTicketQuery, bool>> searchQuery) where TCustomFieldObject : class
        {
            var searchQueryExp = FreshdeskTicketSearchQueryBuilder.Build(searchQuery);
            return await SearchAsync<TCustomFieldObject>(searchQueryExp).ConfigureAwait(false);
        }

        public async Task<FreshdeskTicketSearchResults<TCustomFieldObject>> SearchAsync<TCustomFieldObject, TTicketQueryFields>(Expression<Func<TTicketQueryFields, bool>> searchQuery) 
            where TTicketQueryFields : IFreshdeskTicketQuery 
            where TCustomFieldObject : class
        {
            var searchQueryExp = FreshdeskTicketSearchQueryBuilder.Build(searchQuery);
            return await SearchAsync<TCustomFieldObject>(searchQueryExp).ConfigureAwait(false);
        }

        private static string GetListQueryString(FreshdeskTicketListOptions options)
        {
            if (options == null)
            {
                return "";
            }
            var filters = new List<string>();
            if (options.Filter != default(FreshdeskTicketFilter))
            {
                filters.Add($"filter={options.Filter.ToFilterString()}");
            }
            if (options.Email != default(string))
            {
                filters.Add($"email={WebUtility.UrlEncode(options.Email)}");
            }
            if (options.RequesterId != default(ulong?))
            {
                filters.Add($"requester_id={options.RequesterId.Value}");
            }
            if (options.CompanyId != default(ulong?))
            {
                filters.Add($"company_id={options.CompanyId.Value}");
            }
            if (options.UpdatedSince != default(DateTime?))
            {
                filters.Add($"updated_since={options.UpdatedSince.Value:yyyy-MM-dd}");
            }
            filters.Add($"order_by={options.OrderBy.ToOrderByString()}");
            filters.Add($"order_type={options.OrderType.ToString().ToLower()}");
            AddPageFilters(options, filters);
            return $"?{string.Join("&", filters)}";
        }

        public async Task<List<FreshdeskTicketField>> GetFieldsAsync()
        {
            var requestUri = $"{_apiBaseUri}/ticket_fields";
            using (var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                var result = await GetResponseAsync<List<FreshdeskTicketField>>(response).ConfigureAwait(true);
                return result;
            }
        }
    }
}