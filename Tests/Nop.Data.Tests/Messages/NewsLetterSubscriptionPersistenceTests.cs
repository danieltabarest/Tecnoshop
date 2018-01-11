using Nop.Tests;
using NUnit.Framework;

namespace Nop.Data.Tests.Messages
{
    [TestFixture]
    public class NewsletterSubscriptionPersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_nls()
        {
            var nls = this.GetTestNewsletterSubscription();

            var fromDb = SaveAndLoadEntity(this.GetTestNewsletterSubscription());
            fromDb.ShouldNotBeNull();
            fromDb.PropertiesShouldEqual(nls);
        }
    }
}