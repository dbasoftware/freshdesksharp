using System;
using System.Collections.Generic;

namespace DBA.FreshdeskSharp.Models
{
    public abstract class FreshdeskContactBase
    {
        protected FreshdeskContactBase()
        {
            OtherEmails = new List<string>();
            Tags = new List<string>();
            OtherCompanies = new List<FreshdeskOtherCompany>();
        }

        public bool Active { get; set; }
        public string Address { get; set; }
        public FreshdeskAvatar Avatar { get; set; }
        public ulong CompanyId { get; set; }
        public bool ViewAllTickets { get; set; }
        public bool Deleted { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public ulong Id { get; set; }
        public string JobTitle { get; set; }
        public string Language { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public List<string> OtherEmails { get; }
        public string Phone { get; set; }
        public List<string> Tags { get; }
        public string TimeZone { get; set; }
        public string TwitterId { get; set; }
        public List<FreshdeskOtherCompany> OtherCompanies { get; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}