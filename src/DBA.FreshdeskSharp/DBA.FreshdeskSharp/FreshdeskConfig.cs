using System;
using DBA.FreshdeskSharp.Models.Abstractions;

namespace DBA.FreshdeskSharp
{
    public struct FreshdeskConfig
    {
        public string Domain { get; set; }
        public FreshdeskCredentials Credentials { get; set; }
        public bool MultiCompanySupport { get; set; }
        public bool MultiLanguageSupport { get; set; }
        public bool MultiTimeZoneSupport { get; set; }
        public IFreshdeskClientLogger Logger { get; set; }
        public bool RetryWhenThrottled { get; set; }
        public TimeSpan? Timeout { get; set; }
    }
}