using System;

namespace DBA.FreshdeskSharp.Models.Internal
{
    internal static class FreshdeskTicketListOptionsExtensions
    {
        public static string ToFilterString(this FreshdeskTicketFilter filter)
        {
            switch (filter)
            {
                case FreshdeskTicketFilter.NewAndMyOpen:
                    return "new_and_my_open";
                case FreshdeskTicketFilter.None:
                case FreshdeskTicketFilter.Watching:
                case FreshdeskTicketFilter.Spam:
                case FreshdeskTicketFilter.Deleted:
                    return filter.ToString().ToLower();
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        public static string ToOrderByString(this FreshdeskTicketOrderBy orderBy)
        {
            switch (orderBy)
            {
                case FreshdeskTicketOrderBy.CreatedAt:
                    return "created_at";
                case FreshdeskTicketOrderBy.DueBy:
                    return "due_by";
                case FreshdeskTicketOrderBy.UpdatedAt:
                    return "updated_at";
                case FreshdeskTicketOrderBy.Status:
                    return "status";
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy, null);
            }
        }
    }
}