using Nop.Core.Domain.Messages;

namespace Nop.Data.Mapping.Messages
{
    public partial class Boletín informativoSubscriptionMap : NopEntityTypeConfiguration<Boletín informativoSubscription>
    {
        public Boletín informativoSubscriptionMap()
        {
            this.ToTable("Boletín informativoSubscription");
            this.HasKey(nls => nls.Id);

            this.Property(nls => nls.Email).IsRequired().HasMaxLength(255);
        }
    }
}