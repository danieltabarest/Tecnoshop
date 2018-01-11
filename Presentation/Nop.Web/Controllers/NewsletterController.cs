using System;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Factories;
using Nop.Web.Framework;

namespace Nop.Web.Controllers
{
    public partial class NewsletterController : BasePublicController
    {
        private readonly INewsletterModelFactory _NewsletterModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly INewsletterSubscriptionService _NewsletterSubscriptionService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IStoreContext _storeContext;

        private readonly CustomerSettings _customerSettings;

        public NewsletterController(INewsletterModelFactory NewsletterModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext,
            INewsletterSubscriptionService NewsletterSubscriptionService,
            IWorkflowMessageService workflowMessageService,
            IStoreContext storeContext,
            CustomerSettings customerSettings)
        {
            this._NewsletterModelFactory = NewsletterModelFactory;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._NewsletterSubscriptionService = NewsletterSubscriptionService;
            this._workflowMessageService = workflowMessageService;
            this._storeContext = storeContext;
            this._customerSettings = customerSettings;
        }

        [ChildActionOnly]
        public virtual ActionResult NewsletterBox()
        {
            if (_customerSettings.HideNewsletterBlock)
                return Content("");

            var model = _NewsletterModelFactory.PrepareNewsletterBoxModel();
            return PartialView(model);
        }

        //available even when a store is closed
        [StoreClosed(true)]
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SubscribeNewsletter(string email, bool subscribe)
        {
            string result;
            bool success = false;

            if (!CommonHelper.IsValidEmail(email))
            {
                result = _localizationService.GetResource("Newsletter.Email.Wrong");
            }
            else
            {
                email = email.Trim();

                var subscription = _NewsletterSubscriptionService.GetNewsletterSubscriptionByEmailAndStoreId(email, _storeContext.CurrentStore.Id);
                if (subscription != null)
                {
                    if (subscribe)
                    {
                        if (!subscription.Active)
                        {
                            _workflowMessageService.SendNewsletterSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);
                        }
                        result = _localizationService.GetResource("Newsletter.SubscribeEmailSent");
                    }
                    else
                    {
                        if (subscription.Active)
                        {
                            _workflowMessageService.SendNewsletterSubscriptionDeactivationMessage(subscription, _workContext.WorkingLanguage.Id);
                        }
                        result = _localizationService.GetResource("Newsletter.UnsubscribeEmailSent");
                    }
                }
                else if (subscribe)
                {
                    subscription = new NewsletterSubscription
                    {
                        NewsletterSubscriptionGuid = Guid.NewGuid(),
                        Email = email,
                        Active = false,
                        StoreId = _storeContext.CurrentStore.Id,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    _NewsletterSubscriptionService.InsertNewsletterSubscription(subscription);
                    _workflowMessageService.SendNewsletterSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);

                    result = _localizationService.GetResource("Newsletter.SubscribeEmailSent");
                }
                else
                {
                    result = _localizationService.GetResource("Newsletter.UnsubscribeEmailSent");
                }
                success = true;
            }

            return Json(new
            {
                Success = success,
                Result = result,
            });
        }

        //available even when a store is closed
        [StoreClosed(true)]
        public virtual ActionResult SubscriptionActivation(Guid token, bool active)
        {
            var subscription = _NewsletterSubscriptionService.GetNewsletterSubscriptionByGuid(token);
            if (subscription == null)
                return RedirectToRoute("HomePage");

            if (active)
            {
                subscription.Active = true;
                _NewsletterSubscriptionService.UpdateNewsletterSubscription(subscription);
            }
            else
                _NewsletterSubscriptionService.DeleteNewsletterSubscription(subscription);

            var model = _NewsletterModelFactory.PrepareSubscriptionActivationModel(active);
            return View(model);
        }
    }
}
