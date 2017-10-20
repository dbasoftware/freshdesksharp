namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskTicketSearchResults : FreshdeskSearchResults<FreshdeskTicket<FreshdeskCustomFields>>
    {
    }

    public class FreshdeskTicketSearchResults<TCustomFieldObject> : FreshdeskSearchResults<FreshdeskTicket<TCustomFieldObject>> where TCustomFieldObject : class
    {
    }
}
