namespace DBA.FreshdeskSharp.Models.Internal
{
    internal class FreshdeskConfigInternal
    {
        public static FreshdeskConfigInternal FromConfig(FreshdeskConfig config)
        {
            return new FreshdeskConfigInternal
            {
                ApiBaseUri = $"https://{config.Domain}/api/v2",
                MultiCompanySupport = config.MultiCompanySupport,
                MultiLanguageSupport = config.MultiLanguageSupport,
                MultiTimeZoneSupport = config.MultiTimeZoneSupport,
            };
        }

        public string ApiBaseUri { get; private set; }
        public bool MultiCompanySupport { get; private set; }
        public bool MultiLanguageSupport { get; private set; }
        public bool MultiTimeZoneSupport { get; private set; }
    }
}