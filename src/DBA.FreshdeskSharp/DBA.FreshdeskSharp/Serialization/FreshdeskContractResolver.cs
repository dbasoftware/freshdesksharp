using DBA.FreshdeskSharp.Models;
using DBA.FreshdeskSharp.Models.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DBA.FreshdeskSharp.Serialization
{
    internal class FreshdeskContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly List<string> _nonSerializableContactProps;
        private readonly List<string> _nonSerializableCompanyProps;
        private readonly List<string> _nonSerializableTicketProps;
        private readonly Type _contactType = typeof(FreshdeskContactInternalBase<>);
        private readonly Type _companyType = typeof(FreshdeskCompanyBase);
        private readonly Type _ticketType = typeof(FreshdeskTicketBase);
        
        public FreshdeskContractResolver(FreshdeskConfigInternal config)
        {
            _nonSerializableContactProps = GetNonSerializableContactProps(config);
            _nonSerializableCompanyProps = GetNonSerializableCompanyProps();
            _nonSerializableTicketProps = GetNonSerializableTicketProps(config);
        }

        private List<string> GetNonSerializableTicketProps(FreshdeskConfigInternal config)
        {
            var props = new List<string> {
                "id", "deleted", "created_at", "updated_at", "fwd_emails",
                "reply_cc_emails", "to_emails", "fr_escalated", "spam",
                "is_escalated", "description_text", "attachments"
            };
            if (!config.MultiCompanySupport)
            {
                props.Add("company_id");
            }
            return props;
        }

        private static List<string> GetNonSerializableContactProps(FreshdeskConfigInternal config)
        {
            var props = new List<string> {"id", "deleted", "created_at", "updated_at"};
            if (!config.MultiCompanySupport)
            {
                props.Add("other_companies");
            }
            if (!config.MultiLanguageSupport)
            {
                props.Add("language");
            }
            if (!config.MultiTimeZoneSupport)
            {
                props.Add("time_zone");
            }
            return props;
        }

        private static List<string> GetNonSerializableCompanyProps()
        {
            return new List<string> { "id", "deleted", "created_at", "updated_at" };
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var type = property.DeclaringType.IsConstructedGenericType
                ? property.DeclaringType.GetGenericTypeDefinition()
                : property.DeclaringType;
            var isTicketType = IsTypeDerivedFrom(type, _ticketType);
            var containsProp = _nonSerializableTicketProps.Contains(property.PropertyName);
            if (IsTypeDerivedFrom(type, _contactType) && _nonSerializableContactProps.Contains(property.PropertyName) ||
                IsTypeDerivedFrom(type, _companyType) && _nonSerializableCompanyProps.Contains(property.PropertyName) ||
                isTicketType && _nonSerializableTicketProps.Contains(property.PropertyName))
            {
                property.ShouldSerialize = instance => false;
            }
            return property;
        }

        private static bool IsTypeDerivedFrom(Type type, Type baseType)
        {
            do
            {
                if (type == baseType)
                {
                    return true;
                }

                var typeInfo = type.GetTypeInfo();
                type = typeInfo.BaseType;
            } while (type != null);
            return false;
        }
    }
}