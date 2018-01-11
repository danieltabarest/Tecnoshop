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
    public partial class Boletín informativoController : BasePublicController
    {
        private readonly IBoletín informativoModelFactory _Boletín informativoModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IBoletín informativoSubscriptionService _Boletín informativoSubscriptionService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IStoreContext _storeContext;

        private readonly CustomerSettings _customerSettings;

        public Boletín informativoController(IBoletín informativoModelFactory Boletín informativoModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IBoletín informativoSubscriptionService Boletín informativoSubscriptionService,
            IWorkflowMessageService workflowMessageService,
            IStoreContext storeContext,
            CustomerSettings customerSettings)
        {
            this._Boletín informativoModelFactory = Boletín informativoModelFactory;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._Boletín informativoSubscriptionService = Boletín informativoSubscriptionService;
            this._workflowMessageService = workflowMessageService;
            this._storeContext = storeContext;
            this._customerSettings = customerSettings;
        }

        [ChildActionOnly]
        public virtual ActionResult Boletín informativoBox()
        {
            if (_customerSettings.HideBoletín informativoBlock)
                return Content("");

            var model = _Boletín informativoModelFactory.PrepareBoletín informativoBoxModel();
            return PartialView(model);
        }

        //available even when a store is closed
        [StoreClosed(true)]
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SubscribeBoletín informativo(string email, bool subscribe)
        {
            string result;
            bool success = false;

            if (!CommonHelper.IsValidEmail(email))
            {
                result = _localizationService.GetResource("Boletín informativo.Email.Wrong");
            }
            else
            {
                email = email.Trim();

                var subscription = _Boletín informativoSubscriptionService.GetBoletín informativoSubscriptionByEmailAndStoreId(email, _storeContext.CurrentStore.Id);
                if (subscription != null)
                {
                    if (subscribe)
                    {
                        if (!subscription.Active)
                        {
                            _workflowMessageService.SendBoletín informativoSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);
                        }
                        result = _localizationService.GetResource("Boletín informativo.SubscribeEmailSent");
                    }
                    else
                    {
                        if (subscription.Active)
                        {
                            _workflowMessageService.SendBoletín informativoSubscriptionDeactivationMessage(subscription, _workContext.WorkingLanguage.Id);
                        }
                        result = _localizationService.GetResource("Boletín informativo.UnsubscribeEmailSent");
                    }
                }
                else if (subscribe)
                {
                    subscription = new Boletín informativoSubscription
                    {
                        Boletín informativoSubscriptionGuid = Guid.NewGuid(),
                        Email = email,
                        Active = false,
                        StoreId = _storeContext.CurrentStore.Id,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    _Boletín informativoSubscriptionService.InsertBoletín informativoSubscription(subscription);
                    _workflowMessageService.SendBoletín informativoSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);

                    result = _localizationService.GetResource("Boletín informativo.SubscribeEmailSent");
                }
                else
                {
                    result = _localizationService.GetResource("Boletín informativo.UnsubscribeEmailSent");
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
            var subscription = _Boletín informativoSubscriptionService.GetBoletín informativoSubscriptionByGuid(token);
            if (subscription == null)
                return RedirectToRoute("HomePage");

            if (active)
            {
                subscription.Active = true;
                _Boletín informativoSubscriptionService.UpdateBoletín informativoSubscription(subscription);
            }
            else
                _Boletín informativoSubscriptionService.DeleteBoletín informativoSubscription(subscription);

            var model = _Boletín informativoModelFactory.PrepareSubscriptionActivationModel(active);
            return View(model);
        }
    }
}
