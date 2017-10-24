# FreshdeskSharp #

FreshdeskSharp is a .NET wrapper for the Freshdesk API (v2).  It is written using .NET Standard 1.6, which means FreshdeskSharp can be used be used in .NET projects written in C#, F#, and VB.NET with the full .NET framework on Windows, with Xamarin for iOS or Android, or in a .NET Core project on Windows, Linux, or Mac OS.  This library was initially created by [Greg Cobb](http://www.developerstome.com) in order to meet the needs of DBA Software in supporting our [DBA Manufacturing](https://www.dbamanufacturing.com) customers.  Because of this, at this time it does not fully cover the entire Freshdesk API surface.  If you find yourself wanting to use this library, but need additional API endpoints to be added to be able to do so, please put in an issue and I will try my best to accommodate you.  Hopefully the documentation and examples below are enough to get you started, but if you need more help understanding how the library works feel free to put in an issue requesting help.

## Usage ##

### Configuration ###

For credentials you can either use password:

```cs
var credentials = new FreshdeskCredentials("myusername", "myPassword");
```

or your API key:

```cs
var credentials = new FreshdeskCredentials("MY_API_KEY");
```

The Freshdesk client requires a FreshdeskConfig to specify your configuration:

```cs
var config = new FreshdeskConfig
{
    Domain = "YOUR_FRESHDESK_DOMAIN",
    Credentials = credentials
};
```

The Freshdesk API has different throttling restrictions depending on which plan you are paying for.  Once you have made more requests than the allotted requests per hour, you will receive an error on subsequent requests until enough time has passed.  If you would like the client, when encountering an error response, to wait for the required amount of time then retry the request instead of throwing an error, you can turn on the ```RetryWhenThrottled``` setting in the config by doing the following:

```cs
var config = new FreshdeskConfig
{
    Domain = "YOUR_FRESHDESK_DOMAIN",
    Credentials = credentials,
    RetryWhenThrottled = true
};
```

If you specify a ```Timeout```, the requests will adhere to the specified ```Timeout```.  When ```RetryWhenThrottled``` is set to true and there is also a ```Timeout``` specified, then it will only wait to retry a throttled request if the amount of time that you need to wait before retrying is less than the specified ```Timeout```.  The following will wait to retry the request only for a maximum of 1 minute.  If the initial failed API request informs the client that it needs to wait longer than 1 minute it will immediately throw an error instead of waiting to retry.

```cs
var config = new FreshdeskConfig
{
    Domain = "YOUR_FRESHDESK_DOMAIN",
    Credentials = credentials,
    RetryWhenThrottled = true,
    Timeout = new Timespan(0, 1, 0)
};
```

### Intializing The Client ###

Instantiate the client passing in the desired configuration:

```cs
using (var client = new FreshdeskClient(config))
{
    //Client code here
}
```

### An Example ###

The Freshdesk endpoints are exposed via properties on the client.  All endpoint calls are done through asynchronous calls.  For example if you wanted to get a list of contacts you could do the following:

```cs
using (var client = new FreshdeskClient(config))
{
    var result = await client.Contacts.GetListAsync();
}
```

### List Filters ###

When getting lists you usually have a number of filters you can provide as well as pagination values.  For example:


```cs
using (var client = new FreshdeskClient(config))
{
    var listOptions = new FreshdeskContactListOptions()
    {
        CompanyId = 1234567,
        Page = 1,
        PerPage = 100
    };
    var result = await client.Contacts.GetListAsync(listOptions);
}
```

### Using Without Custom Fields ###

Most endpoints have the ability to get a list of resources or a single resource, create a resource, update a resource, and delete a resource.  Some endpoints include additional functionality such as search or undelete.  By default the endpoint methods will accept and return the basic standard resources.  To create a Company in Freshdesk you could do the following:

```cs
using (var client = new FreshdeskClient(config))
{
    var companyToCreate = new FreshdeskCompany
    {
        Name = "Fake Company Name, Inc."
    };
    var createdCompany = await client.Companies.CreateAsync(companyToCreate);
}
```

### Using With Custom Fields ###

It is likely though that you have added custom fields to Freshdesk to meet your needs.  In that case you should create a custom fields class to represent your custom fields.  Let's say that you have a custom field for Companies in Freshdesk called My Text Field.  You could create a class that represents this data by doing the following:

```cs
public class MyCustomCompanyFields
{
    [JsonProperty("my_text_field")]
    public string MyTextField { get; set; }
}
```
You may notice the JsonProperty attribute (from the Json.NET library) which allows you to use a friendly property name of MyTextField while providing the Freskdesk API with the field name it actually expects which is my_text_field.  In Freshdesk custom fields with spaces in the name replace the spaces with underscores and the custom fields are always lowercased.  If you need help determining the field name that Freskdesk is expecting, you can always call the fields method on the endpoint such as ```client.Companies.GetFieldsAsync``` for the Company field list. 

Now you can create the Company as before but this time with your custom field values:

```cs
using (var client = new FreshdeskClient(config))
{
    var companyToCreate = new FreshdeskCompany<MyCustomCompanyFields>
    {
        Name = "Fake Company Name, Inc.",
        CustomFields = new MyCustomCompanyFields
        {
            MyTextField = "This is a custom field"
        }
    };
    var createdCompany = await client.Companies.CreateAsync(companyToCreate);
}
```

Note that instead of using the standard FreshdeskCompany class to represent the Company resource to be created, we are instead using an instance of FreshdeskCompany<MyCustomCompanyFields> which tells the Freshdesk client to use your MyCustomCompanyFields class to represent the custom fields on the Company resource when creating the record and it will also return the created company with the same format.  C# can infer the generic signature on CreateAsync method calls because the class you are passing in to the method as a parameter provides enough information for the compiler to know what types you want to use.  This is also true of UpdateAsync method calls.  For ```GetAsync``` and ```GetListAsync``` method calls though, you will need to explictly specify the custom field signature you want to use otherwise it will return the standard format.  So for instance to get the company with id 123456 using our custom field format specify above we would do the following:

```cs
using (var client = new FreshdeskClient(config))
{
    var company = await client.Companies.GetAsync<MyCustomCompanyFields>(123456);
}
```

### Search ###

Along with the standard endpoint methods, the Tickets endpoint also has a search method for searching for tickets.  You can either using the string-based custom query format defined in the [API documentation](https://developers.freshdesk.com/api/#filter_tickets).  For instance if you wanted to search for tickets with the ticket status of Open or Pending you could do the following:

```cs
using (var client = new FreshdeskClient(config))
{
    var ticketSearchResults = await client.Tickets.SearchAsync("status:2 OR status:3");
}
```

The search method also allows you to use C# expressions to define the search query.  The C# expression will then be converted to the Freshdesk query format behind the scenes for you.  To use C# expressions to the the same query as above you could do the following:

```cs
using (var client = new FreshdeskClient(config))
{
    var ticketSearchResults = await client.Tickets.SearchAsync(
        t => t.Status == FreshdeskTicketStatus.Open || t.Status == FreshdeskTicketStatus.Pending);
}
```

If you have custom ticket fields and would like to search those fields using the expression format you will need to inform the client library of the format you want to store those custom fields in like shown previously as well as the format of the fields in the query expression.  Since custom query field format is only used in the expression passed to the search method and never needs to be instanciated, you may define the custom format as an interface instead of a class which simplifies your code since you need it to implement the IFreshdeskTicketQuery interface as well.  The following shows using the same custom field format as above in addition to defining the query format so that the custom field can be queried as well using the expression syntax:

```cs
public class MyCustomTicketFields
{
    [JsonProperty("my_text_field")]
    public string MyTextField { get; set; }
}

public interface IMyCustomCompanyFieldsQuery : IFreshdeskTicketQuery
{
    [JsonProperty("my_text_field")]
    string MyTextField { get; set; }
}
```

Now you can search your custom field using the expression syntax as well as have the search return results using that custom field format by doing the following:


```cs
using (var client = new FreshdeskClient(config))
{
    var ticketSearchResults = await client.Tickets.SearchAsync<MyCustomCompanyFields, IMyCustomCompanyFieldsQuery>(t => t.MyTextField == "search for this");
}
```