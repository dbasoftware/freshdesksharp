namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskOutboundEmail : FreshdeskOutboundEmail<FreshdeskCustomFields>
    {
        public FreshdeskOutboundEmail()
        {
            CustomFields = new FreshdeskCustomFields();
        }
    }

    public class FreshdeskOutboundEmail<TCustomFieldObject> : FreshdeskOutboundEmailBase, IFreshdeskCustomFields<TCustomFieldObject> where TCustomFieldObject : class
    {
        public TCustomFieldObject CustomFields { get; set; }
    }
}
