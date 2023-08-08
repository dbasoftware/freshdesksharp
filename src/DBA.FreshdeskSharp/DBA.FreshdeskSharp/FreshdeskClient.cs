using System;
using DBA.FreshdeskSharp.Core;
using DBA.FreshdeskSharp.Endpoints;
using DBA.FreshdeskSharp.Models.Internal;
using DBA.FreshdeskSharp.Serialization;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp
{
    public class FreshdeskClient : IDisposable
    {
        private readonly FreshdeskHttpClient _httpClient;

        public FreshdeskClient(FreshdeskConfig config)
        {
            _httpClient = InitHttpClient(config);
            InitEndpoints(FreshdeskConfigInternal.FromConfig(config));
        }

        private static FreshdeskHttpClient InitHttpClient(FreshdeskConfig config)
        {
            return new FreshdeskHttpClient(config);
        }

        private void InitEndpoints(FreshdeskConfigInternal config)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new FreshdeskContractResolver(config),
                
            };
            Contacts = new FreshdeskContactsEndpoint(config, _httpClient, settings);
            Companies = new FreshdeskCompaniesEndpoint(config, _httpClient, settings);
            Tickets = new FreshdeskTicketsEndpoint(config, _httpClient, settings);
            Agents = new FreshdeskAgentsEndpoint(config, _httpClient, settings);
            CannedResponses = new FreshdeskCannedResponsesEndpoint(config, _httpClient, settings);
        }

        public FreshdeskContactsEndpoint Contacts { get; private set; }

        public FreshdeskCompaniesEndpoint Companies { get; private set; }

        public FreshdeskTicketsEndpoint Tickets { get; private set; }

        public FreshdeskAgentsEndpoint Agents { get; private set; }

        public FreshdeskCannedResponsesEndpoint CannedResponses { get; private set; }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
