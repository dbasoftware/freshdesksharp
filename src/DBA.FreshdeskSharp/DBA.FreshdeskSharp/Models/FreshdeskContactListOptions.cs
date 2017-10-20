namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskContactListOptions : FreshdeskListOptionsBase
    {
        public string Email { get; set; }
        public ulong? Phone { get; set; }
        public ulong? Mobile { get; set; }
        public ulong? CompanyId { get; set; }
        public FreshdeskContactState? State { get; set; }
    }
}