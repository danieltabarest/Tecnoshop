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
    /// Bolet�n informativo subscription service
    /// </summary>
    public class Bolet�n informativoSubscriptionService : IBolet�n informativoSubscriptionService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IDbContext _context;
        private readonly IRepository<Bolet�n informativoSubscription> _subscriptionRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public Bolet�n informativoSubscriptionService(IDbContext context,
            IRepository<Bolet�n informativoSubscription> subscriptionRepository,
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
        /// <param name="subscription">The Bolet�n informativo subscription.</param>
        /// <param name="isSubscribe">if set to <c>true</c> [is subscribe].</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        private void PublishSubscriptionEvent(Bolet�n informativoSubscription subscription, bool isSubscribe, bool publishSubscriptionEvents)
        {
            if (publishSubscriptionEvents)
            {
                if (isSubscribe)
                {
                    _eventPublisher.PublishBolet�n informativoSubscribe(subscription);
                }
                else
                {
                    _eventPublisher.PublishBolet�n informativoUnsubscribe(subscription);
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Inserts a Bolet�n informativo subscription
        /// </summary>
        /// <param name="Bolet�n informativoSubscription">Bolet�n informativo subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void InsertBolet�n informativoSubscription(Bolet�n informativoSubscription Bolet�n informativoSubscription, bool publishSubscriptionEvents = true)
        {
            if (Bolet�n informativoSubscription == null)
            {
                throw new ArgumentNullException("Bolet�n informativoSubscription");
            }

            //Handle e-mail
            Bolet�n informativoSubscription.Email = CommonHelper.EnsureSubscriberEmailOrThrow(Bolet�n informativoSubscription.Email);

            //Persist
            _subscriptionRepository.Insert(Bolet�n informativoSubscription);

            //Publish the subscription event 
            if (Bolet�n informativoSubscription.Active)
            {
                PublishSubscriptionEvent(Bolet�n informativoSubscription, true, publishSubscriptionEvents);
            }

            //Publish event
            _eventPublisher.EntityInserted(Bolet�n informativoSubscription);
        }

        /// <summary>
        /// Updates a Bolet�n informativo subscription
        /// </summary>
        /// <param name="Bolet�n informativoSubscription">Bolet�n informativo subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void UpdateBolet�n informativoSubscription(Bolet�n informativoSubscription Bolet�n informativoSubscription, bool publishSubscriptionEvents = true)
        {
            if (Bolet�n informativoSubscription == null)
            {
                throw new ArgumentNullException("Bolet�n informativoSubscription");
            }

            //Handle e-mail
            Bolet�n informativoSubscription.Email = CommonHelper.EnsureSubscriberEmailOrThrow(Bolet�n informativoSubscription.Email);

            //Get original subscription record
            var originalSubscription = _context.LoadOriginalCopy(Bolet�n informativoSubscription);

            //Persist
            _subscriptionRepository.Update(Bolet�n informativoSubscription);

            //Publish the subscription event 
            if ((originalSubscription.Active == false && Bolet�n informativoSubscription.Active) ||
                (Bolet�n informativoSubscription.Active && (originalSubscription.Email != Bolet�n informativoSubscription.Email)))
            {
                //If the previous entry was false, but this one is true, publish a subscribe.
                PublishSubscriptionEvent(Bolet�n informativoSubscription, true, publishSubscriptionEvents);
            }
            
            if ((originalSubscription.Active && Bolet�n informativoSubscription.Active) && 
                (originalSubscription.Email != Bolet�n informativoSubscription.Email))
            {
                //If the two emails are different publish an unsubscribe.
                PublishSubscriptionEvent(originalSubscription, false, publishSubscriptionEvents);
            }

            if ((originalSubscription.Active && !Bolet�n informativoSubscription.Active))
            {
                //If the previous entry was true, but this one is false
                PublishSubscriptionEvent(originalSubscription, false, publishSubscriptionEvents);
            }

            //Publish event
            _eventPublisher.EntityUpdated(Bolet�n informativoSubscription);
        }

        /// <summary>
        /// Deletes a Bolet�n informativo subscription
        /// </summary>
        /// <param name="Bolet�n informativoSubscription">Bolet�n informativo subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void DeleteBolet�n informativoSubscription(Bolet�n informativoSubscription Bolet�n informativoSubscription, bool publishSubscriptionEvents = true)
        {
            if (Bolet�n informativoSubscription == null) throw new ArgumentNullException("Bolet�n informativoSubscription");

            _subscriptionRepository.Delete(Bolet�n informativoSubscription);

            //Publish the unsubscribe event 
            PublishSubscriptionEvent(Bolet�n informativoSubscription, false, publishSubscriptionEvents);

            //event notification
            _eventPublisher.EntityDeleted(Bolet�n informativoSubscription);
        }

        /// <summary>
        /// Gets a Bolet�n informativo subscription by Bolet�n informativo subscription identifier
        /// </summary>
        /// <param name="Bolet�n informativoSubscriptionId">The Bolet�n informativo subscription identifier</param>
        /// <returns>Bolet�n informativo subscription</returns>
        public virtual Bolet�n informativoSubscription GetBolet�n informativoSubscriptionById(int Bolet�n informativoSubscriptionId)
        {
            if (Bolet�n informativoSubscriptionId == 0) return null;

            return _subscriptionRepository.GetById(Bolet�n informativoSubscriptionId);
        }

        /// <summary>
        /// Gets a Bolet�n informativo subscription by Bolet�n informativo subscription GUID
        /// </summary>
        /// <param name="Bolet�n informativoSubscriptionGuid">The Bolet�n informativo subscription GUID</param>
        /// <returns>Bolet�n informativo subscription</returns>
        public virtual Bolet�n informativoSubscription GetBolet�n informativoSubscriptionByGuid(Guid Bolet�n informativoSubscriptionGuid)
        {
            if (Bolet�n informativoSubscriptionGuid == Guid.Empty) return null;

            var Bolet�n informativoSubscriptions = from nls in _subscriptionRepository.Table
                                          where nls.Bolet�n informativoSubscriptionGuid == Bolet�n informativoSubscriptionGuid
                                          orderby nls.Id
                                          select nls;

            return Bolet�n informativoSubscriptions.FirstOrDefault();
        }

        /// <summary>
        /// Gets a Bolet�n informativo subscription by email and store ID
        /// </summary>
        /// <param name="email">The Bolet�n informativo subscription email</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Bolet�n informativo subscription</returns>
        public virtual Bolet�n informativoSubscription GetBolet�n informativoSubscriptionByEmailAndStoreId(string email, int storeId)
        {
            if (!CommonHelper.IsValidEmail(email)) 
                return null;

            email = email.Trim();

            var Bolet�n informativoSubscriptions = from nls in _subscriptionRepository.Table
                                          where nls.Email == email && nls.StoreId == storeId
                                          orderby nls.Id
                                          select nls;

            return Bolet�n informativoSubscriptions.FirstOrDefault();
        }

        /// <summary>
        /// Gets the Bolet�n informativo subscription list
        /// </summary>
        /// <param name="email">Email to search or string. Empty to load all records.</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="storeId">Store identifier. 0 to load all records.</param>
        /// <param name="customerRoleId">Customer role identifier. Used to filter subscribers by customer role. 0 to load all records.</param>
        /// <param name="isActive">Value indicating whether subscriber record should be active or not; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Bolet�n informativoSubscription entities</returns>
        public virtual IPagedList<Bolet�n informativoSubscription> GetAllBolet�n informativoSubscriptions(string email = null,
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

                var subscriptions = new PagedList<Bolet�n informativoSubscription>(query, pageIndex, pageSize);
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
                    
                    var subscriptions = new PagedList<Bolet�n informativoSubscription>(query, pageIndex, pageSize);
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
                            Bolet�n informativoSubscribers = nls,
                            Customer = c
                        });
                    query = query.Where(x => x.Customer.CustomerRoles.Any(cr => cr.Id == customerRoleId));
                    if (!String.IsNullOrEmpty(email))
                        query = query.Where(x => x.Bolet�n informativoSubscribers.Email.Contains(email));
                    if (createdFromUtc.HasValue)
                        query = query.Where(x => x.Bolet�n informativoSubscribers.CreatedOnUtc >= createdFromUtc.Value);
                    if (createdToUtc.HasValue)
                        query = query.Where(x => x.Bolet�n informativoSubscribers.CreatedOnUtc <= createdToUtc.Value);
                    if (storeId > 0)
                        query = query.Where(x => x.Bolet�n informativoSubscribers.StoreId == storeId);
                    if (isActive.HasValue)
                        query = query.Where(x => x.Bolet�n informativoSubscribers.Active == isActive.Value);
                    query = query.OrderBy(x => x.Bolet�n informativoSubscribers.Email);

                    var subscriptions = new PagedList<Bolet�n informativoSubscription>(query.Select(x=>x.Bolet�n informativoSubscribers), pageIndex, pageSize);
                    return subscriptions;
                }
            }
        }

        #endregion
    }
}