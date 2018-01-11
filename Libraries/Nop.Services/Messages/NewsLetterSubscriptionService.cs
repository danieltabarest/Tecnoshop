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
    /// Boletín informativo subscription service
    /// </summary>
    public class Boletín informativoSubscriptionService : IBoletín informativoSubscriptionService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IDbContext _context;
        private readonly IRepository<Boletín informativoSubscription> _subscriptionRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public Boletín informativoSubscriptionService(IDbContext context,
            IRepository<Boletín informativoSubscription> subscriptionRepository,
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
        /// <param name="subscription">The Boletín informativo subscription.</param>
        /// <param name="isSubscribe">if set to <c>true</c> [is subscribe].</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        private void PublishSubscriptionEvent(Boletín informativoSubscription subscription, bool isSubscribe, bool publishSubscriptionEvents)
        {
            if (publishSubscriptionEvents)
            {
                if (isSubscribe)
                {
                    _eventPublisher.PublishBoletín informativoSubscribe(subscription);
                }
                else
                {
                    _eventPublisher.PublishBoletín informativoUnsubscribe(subscription);
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Inserts a Boletín informativo subscription
        /// </summary>
        /// <param name="Boletín informativoSubscription">Boletín informativo subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void InsertBoletín informativoSubscription(Boletín informativoSubscription Boletín informativoSubscription, bool publishSubscriptionEvents = true)
        {
            if (Boletín informativoSubscription == null)
            {
                throw new ArgumentNullException("Boletín informativoSubscription");
            }

            //Handle e-mail
            Boletín informativoSubscription.Email = CommonHelper.EnsureSubscriberEmailOrThrow(Boletín informativoSubscription.Email);

            //Persist
            _subscriptionRepository.Insert(Boletín informativoSubscription);

            //Publish the subscription event 
            if (Boletín informativoSubscription.Active)
            {
                PublishSubscriptionEvent(Boletín informativoSubscription, true, publishSubscriptionEvents);
            }

            //Publish event
            _eventPublisher.EntityInserted(Boletín informativoSubscription);
        }

        /// <summary>
        /// Updates a Boletín informativo subscription
        /// </summary>
        /// <param name="Boletín informativoSubscription">Boletín informativo subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void UpdateBoletín informativoSubscription(Boletín informativoSubscription Boletín informativoSubscription, bool publishSubscriptionEvents = true)
        {
            if (Boletín informativoSubscription == null)
            {
                throw new ArgumentNullException("Boletín informativoSubscription");
            }

            //Handle e-mail
            Boletín informativoSubscription.Email = CommonHelper.EnsureSubscriberEmailOrThrow(Boletín informativoSubscription.Email);

            //Get original subscription record
            var originalSubscription = _context.LoadOriginalCopy(Boletín informativoSubscription);

            //Persist
            _subscriptionRepository.Update(Boletín informativoSubscription);

            //Publish the subscription event 
            if ((originalSubscription.Active == false && Boletín informativoSubscription.Active) ||
                (Boletín informativoSubscription.Active && (originalSubscription.Email != Boletín informativoSubscription.Email)))
            {
                //If the previous entry was false, but this one is true, publish a subscribe.
                PublishSubscriptionEvent(Boletín informativoSubscription, true, publishSubscriptionEvents);
            }
            
            if ((originalSubscription.Active && Boletín informativoSubscription.Active) && 
                (originalSubscription.Email != Boletín informativoSubscription.Email))
            {
                //If the two emails are different publish an unsubscribe.
                PublishSubscriptionEvent(originalSubscription, false, publishSubscriptionEvents);
            }

            if ((originalSubscription.Active && !Boletín informativoSubscription.Active))
            {
                //If the previous entry was true, but this one is false
                PublishSubscriptionEvent(originalSubscription, false, publishSubscriptionEvents);
            }

            //Publish event
            _eventPublisher.EntityUpdated(Boletín informativoSubscription);
        }

        /// <summary>
        /// Deletes a Boletín informativo subscription
        /// </summary>
        /// <param name="Boletín informativoSubscription">Boletín informativo subscription</param>
        /// <param name="publishSubscriptionEvents">if set to <c>true</c> [publish subscription events].</param>
        public virtual void DeleteBoletín informativoSubscription(Boletín informativoSubscription Boletín informativoSubscription, bool publishSubscriptionEvents = true)
        {
            if (Boletín informativoSubscription == null) throw new ArgumentNullException("Boletín informativoSubscription");

            _subscriptionRepository.Delete(Boletín informativoSubscription);

            //Publish the unsubscribe event 
            PublishSubscriptionEvent(Boletín informativoSubscription, false, publishSubscriptionEvents);

            //event notification
            _eventPublisher.EntityDeleted(Boletín informativoSubscription);
        }

        /// <summary>
        /// Gets a Boletín informativo subscription by Boletín informativo subscription identifier
        /// </summary>
        /// <param name="Boletín informativoSubscriptionId">The Boletín informativo subscription identifier</param>
        /// <returns>Boletín informativo subscription</returns>
        public virtual Boletín informativoSubscription GetBoletín informativoSubscriptionById(int Boletín informativoSubscriptionId)
        {
            if (Boletín informativoSubscriptionId == 0) return null;

            return _subscriptionRepository.GetById(Boletín informativoSubscriptionId);
        }

        /// <summary>
        /// Gets a Boletín informativo subscription by Boletín informativo subscription GUID
        /// </summary>
        /// <param name="Boletín informativoSubscriptionGuid">The Boletín informativo subscription GUID</param>
        /// <returns>Boletín informativo subscription</returns>
        public virtual Boletín informativoSubscription GetBoletín informativoSubscriptionByGuid(Guid Boletín informativoSubscriptionGuid)
        {
            if (Boletín informativoSubscriptionGuid == Guid.Empty) return null;

            var Boletín informativoSubscriptions = from nls in _subscriptionRepository.Table
                                          where nls.Boletín informativoSubscriptionGuid == Boletín informativoSubscriptionGuid
                                          orderby nls.Id
                                          select nls;

            return Boletín informativoSubscriptions.FirstOrDefault();
        }

        /// <summary>
        /// Gets a Boletín informativo subscription by email and store ID
        /// </summary>
        /// <param name="email">The Boletín informativo subscription email</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Boletín informativo subscription</returns>
        public virtual Boletín informativoSubscription GetBoletín informativoSubscriptionByEmailAndStoreId(string email, int storeId)
        {
            if (!CommonHelper.IsValidEmail(email)) 
                return null;

            email = email.Trim();

            var Boletín informativoSubscriptions = from nls in _subscriptionRepository.Table
                                          where nls.Email == email && nls.StoreId == storeId
                                          orderby nls.Id
                                          select nls;

            return Boletín informativoSubscriptions.FirstOrDefault();
        }

        /// <summary>
        /// Gets the Boletín informativo subscription list
        /// </summary>
        /// <param name="email">Email to search or string. Empty to load all records.</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="storeId">Store identifier. 0 to load all records.</param>
        /// <param name="customerRoleId">Customer role identifier. Used to filter subscribers by customer role. 0 to load all records.</param>
        /// <param name="isActive">Value indicating whether subscriber record should be active or not; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Boletín informativoSubscription entities</returns>
        public virtual IPagedList<Boletín informativoSubscription> GetAllBoletín informativoSubscriptions(string email = null,
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

                var subscriptions = new PagedList<Boletín informativoSubscription>(query, pageIndex, pageSize);
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
                    
                    var subscriptions = new PagedList<Boletín informativoSubscription>(query, pageIndex, pageSize);
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
                            Boletín informativoSubscribers = nls,
                            Customer = c
                        });
                    query = query.Where(x => x.Customer.CustomerRoles.Any(cr => cr.Id == customerRoleId));
                    if (!String.IsNullOrEmpty(email))
                        query = query.Where(x => x.Boletín informativoSubscribers.Email.Contains(email));
                    if (createdFromUtc.HasValue)
                        query = query.Where(x => x.Boletín informativoSubscribers.CreatedOnUtc >= createdFromUtc.Value);
                    if (createdToUtc.HasValue)
                        query = query.Where(x => x.Boletín informativoSubscribers.CreatedOnUtc <= createdToUtc.Value);
                    if (storeId > 0)
                        query = query.Where(x => x.Boletín informativoSubscribers.StoreId == storeId);
                    if (isActive.HasValue)
                        query = query.Where(x => x.Boletín informativoSubscribers.Active == isActive.Value);
                    query = query.OrderBy(x => x.Boletín informativoSubscribers.Email);

                    var subscriptions = new PagedList<Boletín informativoSubscription>(query.Select(x=>x.Boletín informativoSubscribers), pageIndex, pageSize);
                    return subscriptions;
                }
            }
        }

        #endregion
    }
}