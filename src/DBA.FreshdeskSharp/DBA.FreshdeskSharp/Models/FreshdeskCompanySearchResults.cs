using System;
using System.Collections.Generic;
using System.Text;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskCompanySearchResults : FreshdeskSearchResults<FreshdeskCompany<FreshdeskCustomFields>>
    {
    }

    public class FreshdeskCompanySearchResults<TCustomFieldObject> : FreshdeskSearchResults<FreshdeskCompany<TCustomFieldObject>> where TCustomFieldObject : class
    {
    }
}
