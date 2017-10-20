using DBA.FreshdeskSharp.Models.Abstractions;

namespace DBA.FreshdeskSharp.Models.Internal
{
    internal class FreshdeskContactInternal : FreshdeskContactInternalBase<object>
    {
    }

    internal class FreshdeskContactInternal<TCustomFieldObject> : FreshdeskContactInternalBase<TCustomFieldObject> where TCustomFieldObject : class
    {
    }
}
