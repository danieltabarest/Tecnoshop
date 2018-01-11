using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Data;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Messages;
using NUnit.Framework;
using Rhino.Mocks;

namespace Nop.Services.Tests.Messages 
{
    [TestFixture]
    public class Boletín informativoSubscriptionServiceTests : ServiceTest
    {
        private IEventPublisher _eventPublisher;
        private IRepository<Boletín informativoSubscription> _Boletín informativoSubscriptionRepository;
        private IRepository<Customer> _customerRepository;
        private ICustomerService _customerService;
        private IDbContext _dbContext;

        [SetUp]
        public new void SetUp()
        {
            _eventPublisher = MockRepository.GenerateStub<IEventPublisher>();
            _Boletín informativoSubscriptionRepository = MockRepository.GenerateMock<IRepository<Boletín informativoSubscription>>();
            _customerRepository = MockRepository.GenerateMock<IRepository<Customer>>();
            _customerService = MockRepository.GenerateMock<ICustomerService>();
            _dbContext = MockRepository.GenerateStub<IDbContext>();
        }

        /// <summary>
        /// Verifies the active insert triggers subscribe event.
        /// </summary>
        [Test]
        public void VerifyActiveInsertTriggersSubscribeEvent()
        {
            var service = new Boletín informativoSubscriptionService(_dbContext, _Boletín informativoSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            var subscription = new Boletín informativoSubscription { Active = true, Email = "test@test.com" };
            service.InsertBoletín informativoSubscription(subscription, true);

            _eventPublisher.AssertWasCalled(x => x.Publish(new EmailSubscribedEvent(subscription)));
        }

        /// <summary>
        /// Verifies the delete triggers unsubscribe event.
        /// </summary>
        [Test]
        public void VerifyDeleteTriggersUnsubscribeEvent()
        {
            var service = new Boletín informativoSubscriptionService(_dbContext, _Boletín informativoSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            var subscription = new Boletín informativoSubscription { Active = true, Email = "test@test.com" };
            service.DeleteBoletín informativoSubscription(subscription, true);

            _eventPublisher.AssertWasCalled(x => x.Publish(new EmailUnsubscribedEvent(subscription)));
        }

        /// <summary>
        /// Verifies the email update triggers unsubscribe and subscribe event.
        /// </summary>
        [Test]
        [Ignore("Ignoring until a solution to the IDbContext methods are found. -SRS")]
        public void VerifyEmailUpdateTriggersUnsubscribeAndSubscribeEvent()
        {
            //Prepare the original result
            var originalSubscription = new Boletín informativoSubscription { Active = true, Email = "test@test.com" };
            _Boletín informativoSubscriptionRepository.Stub(m => m.GetById(Arg<object>.Is.Anything)).Return(originalSubscription);

            var service = new Boletín informativoSubscriptionService(_dbContext, _Boletín informativoSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            var subscription = new Boletín informativoSubscription { Active = true, Email = "test@somenewdomain.com" };
            service.UpdateBoletín informativoSubscription(subscription, true);

            _eventPublisher.AssertWasCalled(x => x.Publish(new EmailUnsubscribedEvent(originalSubscription)));
            _eventPublisher.AssertWasCalled(x => x.Publish(new EmailSubscribedEvent(subscription)));
        }

        /// <summary>
        /// Verifies the inactive to active update triggers subscribe event.
        /// </summary>
        [Test]
        [Ignore("Ignoring until a solution to the IDbContext methods are found. -SRS")]
        public void VerifyInactiveToActiveUpdateTriggersSubscribeEvent()
        {
            //Prepare the original result
            var originalSubscription = new Boletín informativoSubscription { Active = false, Email = "test@test.com" };
            _Boletín informativoSubscriptionRepository.Stub(m => m.GetById(Arg<object>.Is.Anything)).Return(originalSubscription);

            var service = new Boletín informativoSubscriptionService(_dbContext, _Boletín informativoSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            var subscription = new Boletín informativoSubscription { Active = true, Email = "test@test.com" };

            service.UpdateBoletín informativoSubscription(subscription, true);

            _eventPublisher.AssertWasCalled(x => x.Publish(new EmailSubscribedEvent(subscription)));
        }

        /// <summary>
        /// Verifies the insert event is fired.
        /// </summary>
        [Test]
        public void VerifyInsertEventIsFired()
        {
            var service = new Boletín informativoSubscriptionService(_dbContext, _Boletín informativoSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            service.InsertBoletín informativoSubscription(new Boletín informativoSubscription { Email = "test@test.com" });

            _eventPublisher.AssertWasCalled(x => x.EntityInserted(Arg<Boletín informativoSubscription>.Is.Anything));
        }

        /// <summary>
        /// Verifies the update event is fired.
        /// </summary>
        [Test]
        [Ignore("Ignoring until a solution to the IDbContext methods are found. -SRS")]
        public void VerifyUpdateEventIsFired()
        {
            //Prepare the original result
            var originalSubscription = new Boletín informativoSubscription { Active = false, Email = "test@test.com" };

            _Boletín informativoSubscriptionRepository.Stub(m => m.GetById(Arg<object>.Is.Anything)).Return(originalSubscription);
            var service = new Boletín informativoSubscriptionService(_dbContext, _Boletín informativoSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            service.UpdateBoletín informativoSubscription(new Boletín informativoSubscription { Email = "test@test.com" });

            _eventPublisher.AssertWasCalled(x => x.EntityUpdated(Arg<Boletín informativoSubscription>.Is.Anything));
        }
    }
}