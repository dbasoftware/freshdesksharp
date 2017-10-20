using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DBA.FreshdeskSharp.Exceptions;
using DBA.FreshdeskSharp.Models;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Endpoints
{
    public abstract class FreshdeskEndpointBase
    {
        protected static async Task<T> GetResponseAsync<T>(HttpResponseMessage response)
        {
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseJson);
            }
            var errorResponse = JsonConvert.DeserializeObject<FreshdeskErrorResponse>(responseJson);
            errorResponse.StatusCode = response.StatusCode;
            if (errorResponse.Description == null)
            {
                errorResponse.Description = response.ReasonPhrase;
            }
            throw new FreshdeskException(errorResponse);
        }

        protected static async Task GetResponseAsync(HttpResponseMessage response)
        {
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            var errorResponse = JsonConvert.DeserializeObject<FreshdeskErrorResponse>(responseJson);
            errorResponse.StatusCode = response.StatusCode;
            throw new FreshdeskException(errorResponse);
        }

        protected static void AddPageFilters(FreshdeskListOptionsBase options, ICollection<string> filters)
        {
            if (options.Page != default(uint?))
            {
                filters.Add($"page={options.Page.Value}");
            }
            if (options.PerPage != default(uint?))
            {
                filters.Add($"per_page={options.PerPage.Value}");
            }
        }

    }
}