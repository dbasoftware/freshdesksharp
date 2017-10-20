using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskOtherCompany
    {
        [JsonProperty("company_id")]
        public ulong? CompanyId { get; set; }
        [JsonProperty("view_all_tickets")]
        public bool ViewAllTickets { get; set; }
    }
}