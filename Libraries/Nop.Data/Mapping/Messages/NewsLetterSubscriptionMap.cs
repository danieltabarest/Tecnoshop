using Nop.Core.Domain.Messages;

namespace Nop.Data.Mapping.Messages
{
    public partial class Bolet�n informativoSubscriptionMap : NopEntityTypeConfiguration<Bolet�n informativoSubscription>
    {
        public Bolet�n informativoSubscriptionMap()
        {
            this.ToTable("Bolet�n informativoSubscription");
            this.HasKey(nls => nls.Id);

            this.Property(nls => nls.Email).IsRequired().HasMaxLength(255);
        }
    }
}