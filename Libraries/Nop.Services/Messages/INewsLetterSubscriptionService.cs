using System;
using Nop.Core;
using Nop.Core.Domain.Messages;

namespace Nop.Services.Messages
{
    /// <summary>
    /// Boletín informativo subscription service interface
    /// </summary>
    public partial interface IBoletín informativoSubscriptionService
    {
        /// <summary>
        /// Inserts a Boletín informativo subscription
        /// </summary>
        /// <param name="Boletín informativoSubscription">Boletín informativo subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        void InsertBoletín informativoSubscription(Boletín informativoSubscription Boletín informativoSubscription, bool publishSubscriptionEvents = true);

        /// <summary>
        /// Updates a Boletín informativo subscription
        /// </summary>
        /// <param name="Boletín informativoSubscription">Boletín informativo subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        void UpdateBoletín informativoSubscription(Boletín informativoSubscription Boletín informativoSubscription, bool publishSubscriptionEvents = true);

        /// <summary>
        /// Deletes a Boletín informativo subscription
        /// </summary>
        /// <param name="Boletín informativoSubscription">Boletín informativo subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        void DeleteBoletín informativoSubscription(Boletín informativoSubscription Boletín informativoSubscription, bool publishSubscriptionEvents = true);

        /// <summary>
        /// Gets a Boletín informativo subscription by Boletín informativo subscription identifier
        /// </summary>
        /// <param name="Boletín informativoSubscriptionId">The Boletín informativo subscription identifier</param>
        /// <returns>Boletín informativo subscription</returns>
        Boletín informativoSubscription GetBoletín informativoSubscriptionById(int Boletín informativoSubscriptionId);

        /// <summary>
        /// Gets a Boletín informativo subscription by Boletín informativo subscription GUID
        /// </summary>
        /// <param name="Boletín informativoSubscriptionGuid">The Boletín informativo subscription GUID</param>
        /// <returns>Boletín informativo subscription</returns>
        Boletín informativoSubscription GetBoletín informativoSubscriptionByGuid(Guid Boletín informativoSubscriptionGuid);

        /// <summary>
        /// Gets a Boletín informativo subscription by email and store ID
        /// </summary>
        /// <param name="email">The Boletín informativo subscription email</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Boletín informativo subscription</returns>
        Boletín informativoSubscription GetBoletín informativoSubscriptionByEmailAndStoreId(string email, int storeId);

        /// <summary>
        /// Gets the Boletín informativo subscription list
        /// </summary>
        /// <param name="email">Email to search or string. Empty to load all records.</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="storeId">Store identifier. 0 to load all records.</param>
        /// <param name="isActive">Value indicating whether subscriber record should be active or not; null to load all records</param>
        /// <param name="customerRoleId">Customer role identifier. Used to filter subscribers by customer role. 0 to load all records.</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Boletín informativoSubscription entities</returns>
        IPagedList<Boletín informativoSubscription> GetAllBoletín informativoSubscriptions(string email = null,
            DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            int storeId = 0, bool? isActive = null, int customerRoleId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
