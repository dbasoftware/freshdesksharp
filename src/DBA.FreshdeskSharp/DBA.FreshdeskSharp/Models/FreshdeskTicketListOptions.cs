using System;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskTicketListOptions : FreshdeskListOptionsBase
    {
        public string Email { get; set; }
        public ulong? RequesterId { get; set; }
        public ulong? CompanyId { get; set; }
        public FreshdeskTicketFilter Filter { get; set; }
        public DateTime? UpdatedSince { get; set; }
        public FreshdeskTicketOrderBy OrderBy { get; set; }
        public FreshdeskOrderType OrderType { get; set; }
    }
}