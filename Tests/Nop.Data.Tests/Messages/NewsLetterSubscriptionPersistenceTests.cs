using Nop.Tests;
using NUnit.Framework;

namespace Nop.Data.Tests.Messages
{
    [TestFixture]
    public class Boletín informativoSubscriptionPersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_nls()
        {
            var nls = this.GetTestBoletín informativoSubscription();

            var fromDb = SaveAndLoadEntity(this.GetTestBoletín informativoSubscription());
            fromDb.ShouldNotBeNull();
            fromDb.PropertiesShouldEqual(nls);
        }
    }
}