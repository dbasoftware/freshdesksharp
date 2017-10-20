namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskCompany : FreshdeskCompany<FreshdeskCustomFields>
    {
        public FreshdeskCompany()
        {
            CustomFields = new FreshdeskCustomFields();
        }
    }

    public class FreshdeskCompany<TCustomFieldObject> : FreshdeskCompanyBase, IFreshdeskCustomFields<TCustomFieldObject> where TCustomFieldObject : class
    {
        public TCustomFieldObject CustomFields { get; set; }
    }
}
