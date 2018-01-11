using System;

namespace Nop.Core.Domain.Messages
{
    /// <summary>
    /// Represents NewsletterSubscription entity
    /// </summary>
    public partial class NewsletterSubscription : BaseEntity
    {       
        /// <summary>
        /// Gets or sets the Newsletter subscription GUID
        /// </summary>
        public Guid NewsletterSubscriptionGuid { get; set; }

        /// <summary>
        /// Gets or sets the subcriber email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether subscription is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the store identifier in which a customer has subscribed to Newsletter
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when subscription was created
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
