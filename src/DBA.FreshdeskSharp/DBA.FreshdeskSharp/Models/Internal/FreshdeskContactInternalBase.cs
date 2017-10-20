using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models.Internal
{
    internal class FreshdeskContactInternalBase<TCustomFieldObject> :  IFreshdeskCustomFields<TCustomFieldObject> where TCustomFieldObject : class
    {
        public bool Active { get; set; }
        public string Address { get; set; }
        public FreshdeskAvatar Avatar { get; set; }
        [JsonProperty("company_id")]
        public ulong? CompanyId { get; set; }
        [JsonProperty("view_all_tickets")]
        public bool? ViewAllTickets { get; set; }
        public bool Deleted { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public ulong Id { get; set; }
        [JsonProperty("job_title")]
        public string JobTitle { get; set; }
        public string Language { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        [JsonProperty("other_emails")]
        public List<string> OtherEmails { get; set; }
        public string Phone { get; set; }
        public List<string> Tags { get; set; }
        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }
        [JsonProperty("twitter_id")]
        public string TwitterId { get; set; }
        [JsonProperty("other_companies")]
        public List<FreshdeskOtherCompany> OtherCompanies { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("custom_fields")]
        public TCustomFieldObject CustomFields { get; set; }

        public FreshdeskContact<TCustomFieldObject> ToContact()
        {
            var contact = new FreshdeskContact<TCustomFieldObject>
            {
                Active = Active,
                Address = Address,
                Avatar = Avatar,
                CompanyId = CompanyId ?? default(ulong),
                ViewAllTickets = ViewAllTickets ?? default(bool),
                Deleted = Deleted,
                Description = Description,
                Email = Email,
                Id = Id,
                JobTitle = JobTitle,
                Language = Language,
                Mobile = Mobile,
                Name = Name,
                Phone = Phone,
                TimeZone = TimeZone,
                TwitterId = TwitterId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CustomFields = CustomFields
            };
            if (OtherEmails != null && OtherEmails.Count > 0)
            {
                foreach (var otherEmail in OtherEmails)
                {
                    contact.OtherEmails.Add(otherEmail);
                }
            }
            if (Tags != null && Tags.Count > 0)
            {
                foreach (var tag in Tags)
                {
                    contact.Tags.Add(tag);
                }
            }
            if (OtherCompanies != null && OtherCompanies.Count > 0)
            {
                foreach (var otherCompany in OtherCompanies)
                {
                    contact.OtherCompanies.Add(otherCompany);
                }
            }
            return contact;
        }

        public static FreshdeskContactInternalBase<TCustomFieldObject> FromContact(FreshdeskContact<TCustomFieldObject> contact)
        {
            ulong? companyId;
            if (contact.CompanyId == default(ulong))
            {
                companyId = null;
            }
            else
            {
                companyId = contact.CompanyId;
            }
            return new FreshdeskContactInternalBase<TCustomFieldObject>
            {
                Active = contact.Active,
                Address = contact.Address,
                Avatar = contact.Avatar,
                CompanyId = companyId,
                ViewAllTickets = contact.ViewAllTickets,
                Deleted = contact.Deleted,
                Description = contact.Description,
                Email = contact.Email,
                Id = contact.Id,
                JobTitle = contact.JobTitle,
                Language = contact.Language,
                Mobile = contact.Mobile,
                Name = contact.Name,
                Phone = contact.Phone,
                TimeZone = contact.TimeZone,
                TwitterId = contact.TwitterId,
                CreatedAt = contact.CreatedAt,
                UpdatedAt = contact.UpdatedAt,
                OtherEmails = contact.OtherEmails,
                Tags = contact.Tags,
                OtherCompanies = contact.OtherCompanies,
                CustomFields = contact.CustomFields
            };
        }
    }
}