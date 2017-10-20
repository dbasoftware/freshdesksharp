using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBA.FreshdeskSharp.Models;
using DBA.FreshdeskSharp.Models.Abstractions;
using DBA.FreshdeskSharp.Serialization;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            const string freshdeskDomain = "YOUR_DOMAIN_NAME.freshdesk.com";
            const string freshdeskApiKey = "YOUR_API_KEY";
            GetAllContactsAsync(freshdeskDomain, freshdeskApiKey).GetAwaiter().GetResult();
        }

        private static async Task GetAllContactsAsync(string domain, string apiKey)
        {
            var config = new FreshdeskConfig
            {
                Domain = domain,
                Credentials = new FreshdeskCredentials(apiKey),
                RetryWhenThrottled = true
            };
            using (var client = new FreshdeskClient(config))
            {
                try
                {
                    var result = await client.Contacts.GetListAsync();
                    var account = await client.Companies.CreateAsync(new FreshdeskCompany
                    {
                        Name = "Fake Company 8, Inc."
                    });
                    var contact = await client.Contacts.CreateAsync(new FreshdeskContact
                    {
                        Active = true,
                        Name = "Mr User 6",
                        Email = "fakeusername7@fakedomain.com",
                        CompanyId = account.Id
                    });
                    var ticket = await client.Tickets.CreateAsync(new FreshdeskTicket()
                    {
                        RequesterId = contact.Id,
                        Subject = "Test",
                        Description = "This is a test issue"
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }

}
