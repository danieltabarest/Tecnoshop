﻿using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Admin.Extensions;
using Nop.Admin.Models.Messages;
using Nop.Core;
using Nop.Services.Customers;
using Nop.Services.ExportImport;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Controllers
{
	public partial class NewsletterSubscriptionController : BaseAdminController
	{
		private readonly INewsletterSubscriptionService _NewsletterSubscriptionService;
		private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IStoreService _storeService;
        private readonly ICustomerService _customerService;
        private readonly IExportManager _exportManager;
        private readonly IImportManager _importManager;

		public NewsletterSubscriptionController(INewsletterSubscriptionService NewsletterSubscriptionService,
			IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IStoreService storeService,
            ICustomerService customerService,
            IExportManager exportManager,
            IImportManager importManager)
		{
			this._NewsletterSubscriptionService = NewsletterSubscriptionService;
			this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._storeService = storeService;
            this._customerService = customerService;
            this._exportManager = exportManager;
            this._importManager = importManager;
		}

		public virtual ActionResult Index()
		{
			return RedirectToAction("List");
		}

		public virtual ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNewsletterSubscribers))
                return AccessDeniedView();

            var model = new NewsletterSubscriptionListModel();

            //stores
            model.AvailableStores.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            //active
            model.ActiveList.Add(new SelectListItem
            {
                Value = "0",
                Text = _localizationService.GetResource("Admin.Promotions.NewsletterSubscriptions.List.SearchActive.All")
            });
            model.ActiveList.Add(new SelectListItem
            {
                Value = "1",
                Text = _localizationService.GetResource("Admin.Promotions.NewsletterSubscriptions.List.SearchActive.ActiveOnly")
            });
            model.ActiveList.Add(new SelectListItem
            {
                Value = "2",
                Text = _localizationService.GetResource("Admin.Promotions.NewsletterSubscriptions.List.SearchActive.NotActiveOnly")
            });

            //customer roles
            model.AvailableCustomerRoles.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var cr in _customerService.GetAllCustomerRoles(true))
                model.AvailableCustomerRoles.Add(new SelectListItem { Text = cr.Name, Value = cr.Id.ToString() });

			return View(model);
		}

		[HttpPost]
		public virtual ActionResult SubscriptionList(DataSourceRequest command, NewsletterSubscriptionListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNewsletterSubscribers))
                return AccessDeniedKendoGridJson();

            bool? isActive = null;
            if (model.ActiveId == 1)
                isActive = true;
            else if (model.ActiveId == 2)
                isActive = false;

            var startDateValue = (model.StartDate == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);
            var endDateValue = (model.EndDate == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            var NewsletterSubscriptions = _NewsletterSubscriptionService.GetAllNewsletterSubscriptions(model.SearchEmail,
                startDateValue, endDateValue, model.StoreId, isActive, model.CustomerRoleId,
                command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = NewsletterSubscriptions.Select(x =>
				{
					var m = x.ToModel();
				    var store = _storeService.GetStoreById(x.StoreId);
				    m.StoreName = store != null ? store.Name : "Unknown store";
					m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
					return m;
				}),
                Total = NewsletterSubscriptions.TotalCount
            };

            return Json(gridModel);
		}

        [HttpPost]
        public virtual ActionResult SubscriptionUpdate([Bind(Exclude = "CreatedOn")] NewsletterSubscriptionModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNewsletterSubscribers))
                return AccessDeniedView();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }

            var subscription = _NewsletterSubscriptionService.GetNewsletterSubscriptionById(model.Id);
            subscription.Email = model.Email;
            subscription.Active = model.Active;
            _NewsletterSubscriptionService.UpdateNewsletterSubscription(subscription);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual ActionResult SubscriptionDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNewsletterSubscribers))
                return AccessDeniedView();

            var subscription = _NewsletterSubscriptionService.GetNewsletterSubscriptionById(id);
            if (subscription == null)
                throw new ArgumentException("No subscription found with the specified id");
            _NewsletterSubscriptionService.DeleteNewsletterSubscription(subscription);

            return new NullJsonResult();
        }

        [HttpPost, ActionName("List")]
        [FormValueRequired("exportcsv")]
		public virtual ActionResult ExportCsv(NewsletterSubscriptionListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNewsletterSubscribers))
                return AccessDeniedView();

            bool? isActive = null;
            if (model.ActiveId == 1)
                isActive = true;
            else if (model.ActiveId == 2)
                isActive = false;

            var startDateValue = (model.StartDate == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);
            var endDateValue = (model.EndDate == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            var subscriptions = _NewsletterSubscriptionService.GetAllNewsletterSubscriptions(model.SearchEmail,
                startDateValue, endDateValue, model.StoreId, isActive, model.CustomerRoleId);

		    string result = _exportManager.ExportNewsletterSubscribersToTxt(subscriptions);

            string fileName = String.Format("Newsletter_emails_{0}_{1}.txt", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), CommonHelper.GenerateRandomDigitCode(4));
			return File(Encoding.UTF8.GetBytes(result), MimeTypes.TextCsv, fileName);
		}

        [HttpPost]
        public virtual ActionResult ImportCsv(FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNewsletterSubscribers))
                return AccessDeniedView();

            try
            {
                var file = Request.Files["importcsvfile"];
                if (file != null && file.ContentLength > 0)
                {
                    int count = _importManager.ImportNewsletterSubscribersFromTxt(file.InputStream);
                    SuccessNotification(String.Format(_localizationService.GetResource("Admin.Promotions.NewsletterSubscriptions.ImportEmailsSuccess"), count));
                    return RedirectToAction("List");
                }
                ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }
	}
}
