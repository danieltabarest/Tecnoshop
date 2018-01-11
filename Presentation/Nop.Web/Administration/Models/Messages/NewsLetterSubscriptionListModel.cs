using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Messages
{
    public partial class NewsletterSubscriptionListModel : BaseNopModel
    {
        public NewsletterSubscriptionListModel()
        {
            AvailableStores = new List<SelectListItem>();
            ActiveList = new List<SelectListItem>();
            AvailableCustomerRoles = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.List.SearchEmail")]
        public string SearchEmail { get; set; }

        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.List.SearchStore")]
        public int StoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.List.SearchActive")]
        public int ActiveId { get; set; }
        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.List.SearchActive")]
        public IList<SelectListItem> ActiveList { get; set; }

        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.List.CustomerRoles")]
        public int CustomerRoleId { get; set; }
        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.List.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.List.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

    }
}