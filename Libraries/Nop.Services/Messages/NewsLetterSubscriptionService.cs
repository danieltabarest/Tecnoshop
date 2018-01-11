using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Data;
using Nop.Services.Customers;
using Nop.Services.Events;

namespace Nop.Services.Messages
{
    /// <summary>
    /// Newsletter subscription service
    /// </summary>
    public class NewsletterSubscriptionService : INewsletterSubscriptionService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IDbContext _context;
        private readonly IRepository<NewsletterSubscription> _subscriptionRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public NewsletterSubscriptionService(IDbContext context,
            IRepository<NewsletterSubscription> subscriptionRepository,
            IRepository<Customer> customerRepository,
            IEventPublisher eventPublisher,
            ICustomerService customerService)
        {
            this._context = context;
            this._subscriptionRepository = subscriptionRepository;
            this._customerRepository = customerRepository;
            this._eventPublisher = eventPublisher;
            this._customerService = customerService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Publishes the subscription event.
        /// </summary>
        /// <param name="subscription">The Newsletter subscription.</param>
        /// <param name="isSubscribe">if set to <c>true</c> [is subscribe].</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        private void PublishSubscriptionEvent(NewsletterSubscription subscription, bool isSubscribe, bool publishSubscriptionEvents)
        {
            if (publishSubscriptionEvents)
            {
                if (isSubscribe)
                {
                    _eventPublisher.PublishNewsletterSubscribe(subscription);
                }
                else
                {
                    _eventPublisher.PublishNewsletterUnsubscribe(subscription);
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Inserts a Newsletter subscription
        /// </summary>
        /// <param name="NewsletterSubscription">Newsletter subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void InsertNewsletterSubscription(NewsletterSubscription NewsletterSubscription, bool publishSubscriptionEvents = true)
        {
            if (NewsletterSubscription == null)
            {
                throw new ArgumentNullException("NewsletterSubscription");
            }

            //Handle e-mail
            NewsletterSubscription.Email = CommonHelper.EnsureSubscriberEmailOrThrow(NewsletterSubscription.Email);

            //Persist
            _subscriptionRepository.Insert(NewsletterSubscription);

            //Publish the subscription event 
            if (NewsletterSubscription.Active)
            {
                PublishSubscriptionEvent(NewsletterSubscription, true, publishSubscriptionEvents);
            }

            //Publish event
            _eventPublisher.EntityInserted(NewsletterSubscription);
        }

        /// <summary>
        /// Updates a Newsletter subscription
        /// </summary>
        /// <param name="NewsletterSubscription">Newsletter subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void UpdateNewsletterSubscription(NewsletterSubscription NewsletterSubscription, bool publishSubscriptionEvents = true)
        {
            if (NewsletterSubscription == null)
            {
                throw new ArgumentNullException("NewsletterSubscription");
            }

            //Handle e-mail
            NewsletterSubscription.Email = CommonHelper.EnsureSubscriberEmailOrThrow(NewsletterSubscription.Email);

            //Get original subscription record
            var originalSubscription = _context.LoadOriginalCopy(NewsletterSubscription);

            //Persist
            _subscriptionRepository.Update(NewsletterSubscription);

            //Publish the subscription event 
            if ((originalSubscription.Active == false && NewsletterSubscription.Active) ||
                (NewsletterSubscription.Active && (originalSubscription.Email != NewsletterSubscription.Email)))
            {
                //If the previous entry was false, but this one is true, publish a subscribe.
                PublishSubscriptionEvent(NewsletterSubscription, true, publishSubscriptionEvents);
            }
            
            if ((originalSubscription.Active && NewsletterSubscription.Active) && 
                (originalSubscription.Email != NewsletterSubscription.Email))
            {
                //If the two emails are different publish an unsubscribe.
                PublishSubscriptionEvent(originalSubscription, false, publishSubscriptionEvents);
            }

            if ((originalSubscription.Active && !NewsletterSubscription.Active))
            {
                //If the previous entry was true, but this one is false
                PublishSubscriptionEvent(originalSubscription, false, publishSubscriptionEvents);
            }

            //Publish event
            _eventPublisher.EntityUpdated(NewsletterSubscription);
        }

        /// <summary>
        /// Deletes a Newsletter subscription
        /// </summary>
        /// <param name="NewsletterSubscription">Newsletter subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void DeleteNewsletterSubscription(NewsletterSubscription NewsletterSubscription, bool publishSubscriptionEvents = true)
        {
            if (NewsletterSubscription == null) throw new ArgumentNullException("NewsletterSubscription");

            _subscriptionRepository.Delete(NewsletterSubscription);

            //Publish the unsubscribe event 
            PublishSubscriptionEvent(NewsletterSubscription, false, publishSubscriptionEvents);

            //event notification
            _eventPublisher.EntityDeleted(NewsletterSubscription);
        }

        /// <summary>
        /// Gets a Newsletter subscription by Newsletter subscription identifier
        /// </summary>
        /// <param name="NewsletterSubscriptionId">The Newsletter subscription identifier</param>
        /// <returns>Newsletter subscription</returns>
        public virtual NewsletterSubscription GetNewsletterSubscriptionById(int NewsletterSubscriptionId)
        {
            if (NewsletterSubscriptionId == 0) return null;

            return _subscriptionRepository.GetById(NewsletterSubscriptionId);
        }

        /// <summary>
        /// Gets a Newsletter subscription by Newsletter subscription GUID
        /// </summary>
        /// <param name="NewsletterSubscriptionGuid">The Newsletter subscription GUID</param>
        /// <returns>Newsletter subscription</returns>
        public virtual NewsletterSubscription GetNewsletterSubscriptionByGuid(Guid NewsletterSubscriptionGuid)
        {
            if (NewsletterSubscriptionGuid == Guid.Empty) return null;

            var NewsletterSubscriptions = from nls in _subscriptionRepository.Table
                                          where nls.NewsletterSubscriptionGuid == NewsletterSubscriptionGuid
                                          orderby nls.Id
                                          select nls;

            return NewsletterSubscriptions.FirstOrDefault();
        }

        /// <summary>
        /// Gets a Newsletter subscription by email and store ID
        /// </summary>
        /// <param name="email">The Newsletter subscription email</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Newsletter subscription</returns>
        public virtual NewsletterSubscription GetNewsletterSubscriptionByEmailAndStoreId(string email, int storeId)
        {
            if (!CommonHelper.IsValidEmail(email)) 
                return null;

            email = email.Trim();

            var NewsletterSubscriptions = from nls in _subscriptionRepository.Table
                                          where nls.Email == email && nls.StoreId == storeId
                                          orderby nls.Id
                                          select nls;

            return NewsletterSubscriptions.FirstOrDefault();
        }

        /// <summary>
        /// Gets the Newsletter subscription list
        /// </summary>
        /// <param name="email">Email to search or string. Empty to load all records.</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="storeId">Store identifier. 0 to load all records.</param>
        /// <param name="customerRoleId">Customer role identifier. Used to filter subscribers by customer role. 0 to load all records.</param>
        /// <param name="isActive">Value indicating whether subscriber record should be active or not; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>NewsletterSubscription entities</returns>
        public virtual IPagedList<NewsletterSubscription> GetAllNewsletterSubscriptions(string email = null,
            DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            int storeId = 0, bool? isActive = null, int customerRoleId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            if (customerRoleId == 0)
            {
                //do not filter by customer role
                var query = _subscriptionRepository.Table;
                if (!String.IsNullOrEmpty(email))
                    query = query.Where(nls => nls.Email.Contains(email));
                if (createdFromUtc.HasValue)
                    query = query.Where(nls => nls.CreatedOnUtc >= createdFromUtc.Value);
                if (createdToUtc.HasValue)
                    query = query.Where(nls => nls.CreatedOnUtc <= createdToUtc.Value);
                if (storeId > 0)
                    query = query.Where(nls => nls.StoreId == storeId);
                if (isActive.HasValue)
                    query = query.Where(nls => nls.Active == isActive.Value);
                query = query.OrderBy(nls => nls.Email);

                var subscriptions = new PagedList<NewsletterSubscription>(query, pageIndex, pageSize);
                return subscriptions;
            }
            else
            {
                //filter by customer role
                var guestRole = _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
                if (guestRole == null)
                    throw new NopException("'Guests' role could not be loaded");

                if (guestRole.Id == customerRoleId)
                {
                    //guests
                    var query = _subscriptionRepository.Table;
                    if (!String.IsNullOrEmpty(email))
                        query = query.Where(nls => nls.Email.Contains(email));
                    if (createdFromUtc.HasValue)
                        query = query.Where(nls => nls.CreatedOnUtc >= createdFromUtc.Value);
                    if (createdToUtc.HasValue)
                        query = query.Where(nls => nls.CreatedOnUtc <= createdToUtc.Value);
                    if (storeId > 0)
                        query = query.Where(nls => nls.StoreId == storeId);
                    if (isActive.HasValue)
                        query = query.Where(nls => nls.Active == isActive.Value);
                    query = query.Where(nls => !_customerRepository.Table.Any(c => c.Email == nls.Email));
                    query = query.OrderBy(nls => nls.Email);
                    
                    var subscriptions = new PagedList<NewsletterSubscription>(query, pageIndex, pageSize);
                    return subscriptions;
                }
                else
                {
                    //other customer roles (not guests)
                    var query = _subscriptionRepository.Table.Join(_customerRepository.Table,
                        nls => nls.Email,
                        c => c.Email,
                        (nls, c) => new
                        {
                            NewsletterSubscribers = nls,
                            Customer = c
                        });
                    query = query.Where(x => x.Customer.CustomerRoles.Any(cr => cr.Id == customerRoleId));
                    if (!String.IsNullOrEmpty(email))
                        query = query.Where(x => x.NewsletterSubscribers.Email.Contains(email));
                    if (createdFromUtc.HasValue)
                        query = query.Where(x => x.NewsletterSubscribers.CreatedOnUtc >= createdFromUtc.Value);
                    if (createdToUtc.HasValue)
                        query = query.Where(x => x.NewsletterSubscribers.CreatedOnUtc <= createdToUtc.Value);
                    if (storeId > 0)
                        query = query.Where(x => x.NewsletterSubscribers.StoreId == storeId);
                    if (isActive.HasValue)
                        query = query.Where(x => x.NewsletterSubscribers.Active == isActive.Value);
                    query = query.OrderBy(x => x.NewsletterSubscribers.Email);

                    var subscriptions = new PagedList<NewsletterSubscription>(query.Select(x=>x.NewsletterSubscribers), pageIndex, pageSize);
                    return subscriptions;
                }
            }
        }

        #endregion
    }
}