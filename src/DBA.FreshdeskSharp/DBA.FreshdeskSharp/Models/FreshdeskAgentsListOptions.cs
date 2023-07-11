namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskAgentsListOptions : FreshdeskListOptionsBase
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
    }
}