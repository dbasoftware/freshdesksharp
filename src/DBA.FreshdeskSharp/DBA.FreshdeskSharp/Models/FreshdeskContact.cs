namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskContact : FreshdeskContact<FreshdeskCustomFields>
    {
        public FreshdeskContact()
        {
            CustomFields = new FreshdeskCustomFields();
        }
    }

    public class FreshdeskContact<TCustomFieldObject> : FreshdeskContactBase, IFreshdeskCustomFields<TCustomFieldObject> where TCustomFieldObject : class
    {
        public TCustomFieldObject CustomFields { get; set; }
    }
}