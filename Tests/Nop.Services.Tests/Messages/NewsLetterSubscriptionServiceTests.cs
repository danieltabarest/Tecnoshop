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
    public class NewsletterSubscriptionServiceTests : ServiceTest
    {
        private IEventPublisher _eventPublisher;
        private IRepository<NewsletterSubscription> _NewsletterSubscriptionRepository;
        private IRepository<Customer> _customerRepository;
        private ICustomerService _customerService;
        private IDbContext _dbContext;

        [SetUp]
        public new void SetUp()
        {
            _eventPublisher = MockRepository.GenerateStub<IEventPublisher>();
            _NewsletterSubscriptionRepository = MockRepository.GenerateMock<IRepository<NewsletterSubscription>>();
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
            var service = new NewsletterSubscriptionService(_dbContext, _NewsletterSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            var subscription = new NewsletterSubscription { Active = true, Email = "test@test.com" };
            service.InsertNewsletterSubscription(subscription, true);

            _eventPublisher.AssertWasCalled(x => x.Publish(new EmailSubscribedEvent(subscription)));
        }

        /// <summary>
        /// Verifies the delete triggers unsubscribe event.
        /// </summary>
        [Test]
        public void VerifyDeleteTriggersUnsubscribeEvent()
        {
            var service = new NewsletterSubscriptionService(_dbContext, _NewsletterSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            var subscription = new NewsletterSubscription { Active = true, Email = "test@test.com" };
            service.DeleteNewsletterSubscription(subscription, true);

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
            var originalSubscription = new NewsletterSubscription { Active = true, Email = "test@test.com" };
            _NewsletterSubscriptionRepository.Stub(m => m.GetById(Arg<object>.Is.Anything)).Return(originalSubscription);

            var service = new NewsletterSubscriptionService(_dbContext, _NewsletterSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            var subscription = new NewsletterSubscription { Active = true, Email = "test@somenewdomain.com" };
            service.UpdateNewsletterSubscription(subscription, true);

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
            var originalSubscription = new NewsletterSubscription { Active = false, Email = "test@test.com" };
            _NewsletterSubscriptionRepository.Stub(m => m.GetById(Arg<object>.Is.Anything)).Return(originalSubscription);

            var service = new NewsletterSubscriptionService(_dbContext, _NewsletterSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            var subscription = new NewsletterSubscription { Active = true, Email = "test@test.com" };

            service.UpdateNewsletterSubscription(subscription, true);

            _eventPublisher.AssertWasCalled(x => x.Publish(new EmailSubscribedEvent(subscription)));
        }

        /// <summary>
        /// Verifies the insert event is fired.
        /// </summary>
        [Test]
        public void VerifyInsertEventIsFired()
        {
            var service = new NewsletterSubscriptionService(_dbContext, _NewsletterSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            service.InsertNewsletterSubscription(new NewsletterSubscription { Email = "test@test.com" });

            _eventPublisher.AssertWasCalled(x => x.EntityInserted(Arg<NewsletterSubscription>.Is.Anything));
        }

        /// <summary>
        /// Verifies the update event is fired.
        /// </summary>
        [Test]
        [Ignore("Ignoring until a solution to the IDbContext methods are found. -SRS")]
        public void VerifyUpdateEventIsFired()
        {
            //Prepare the original result
            var originalSubscription = new NewsletterSubscription { Active = false, Email = "test@test.com" };

            _NewsletterSubscriptionRepository.Stub(m => m.GetById(Arg<object>.Is.Anything)).Return(originalSubscription);
            var service = new NewsletterSubscriptionService(_dbContext, _NewsletterSubscriptionRepository,
                _customerRepository, _eventPublisher, _customerService);

            service.UpdateNewsletterSubscription(new NewsletterSubscription { Email = "test@test.com" });

            _eventPublisher.AssertWasCalled(x => x.EntityUpdated(Arg<NewsletterSubscription>.Is.Anything));
        }
    }
}