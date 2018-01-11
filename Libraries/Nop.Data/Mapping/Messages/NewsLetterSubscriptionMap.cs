using Nop.Core.Domain.Messages;

namespace Nop.Data.Mapping.Messages
{
    public partial class NewsletterSubscriptionMap : NopEntityTypeConfiguration<NewsletterSubscription>
    {
        public NewsletterSubscriptionMap()
        {
            this.ToTable("NewsletterSubscription");
            this.HasKey(nls => nls.Id);

            this.Property(nls => nls.Email).IsRequired().HasMaxLength(255);
        }
    }
}