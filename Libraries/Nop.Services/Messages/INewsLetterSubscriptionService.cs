using System;
using Nop.Core;
using Nop.Core.Domain.Messages;

namespace Nop.Services.Messages
{
    /// <summary>
    /// Newsletter subscription service interface
    /// </summary>
    public partial interface INewsletterSubscriptionService
    {
        /// <summary>
        /// Inserts a Newsletter subscription
        /// </summary>
        /// <param name="NewsletterSubscription">Newsletter subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        void InsertNewsletterSubscription(NewsletterSubscription NewsletterSubscription, bool publishSubscriptionEvents = true);

        /// <summary>
        /// Updates a Newsletter subscription
        /// </summary>
        /// <param name="NewsletterSubscription">Newsletter subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        void UpdateNewsletterSubscription(NewsletterSubscription NewsletterSubscription, bool publishSubscriptionEvents = true);

        /// <summary>
        /// Deletes a Newsletter subscription
        /// </summary>
        /// <param name="NewsletterSubscription">Newsletter subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        void DeleteNewsletterSubscription(NewsletterSubscription NewsletterSubscription, bool publishSubscriptionEvents = true);

        /// <summary>
        /// Gets a Newsletter subscription by Newsletter subscription identifier
        /// </summary>
        /// <param name="NewsletterSubscriptionId">The Newsletter subscription identifier</param>
        /// <returns>Newsletter subscription</returns>
        NewsletterSubscription GetNewsletterSubscriptionById(int NewsletterSubscriptionId);

        /// <summary>
        /// Gets a Newsletter subscription by Newsletter subscription GUID
        /// </summary>
        /// <param name="NewsletterSubscriptionGuid">The Newsletter subscription GUID</param>
        /// <returns>Newsletter subscription</returns>
        NewsletterSubscription GetNewsletterSubscriptionByGuid(Guid NewsletterSubscriptionGuid);

        /// <summary>
        /// Gets a Newsletter subscription by email and store ID
        /// </summary>
        /// <param name="email">The Newsletter subscription email</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Newsletter subscription</returns>
        NewsletterSubscription GetNewsletterSubscriptionByEmailAndStoreId(string email, int storeId);

        /// <summary>
        /// Gets the Newsletter subscription list
        /// </summary>
        /// <param name="email">Email to search or string. Empty to load all records.</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="storeId">Store identifier. 0 to load all records.</param>
        /// <param name="isActive">Value indicating whether subscriber record should be active or not; null to load all records</param>
        /// <param name="customerRoleId">Customer role identifier. Used to filter subscribers by customer role. 0 to load all records.</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>NewsletterSubscription entities</returns>
        IPagedList<NewsletterSubscription> GetAllNewsletterSubscriptions(string email = null,
            DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            int storeId = 0, bool? isActive = null, int customerRoleId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
