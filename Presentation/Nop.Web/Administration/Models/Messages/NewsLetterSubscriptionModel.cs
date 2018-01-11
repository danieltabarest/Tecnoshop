using System;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Messages
{
    [Validator(typeof(NewsletterSubscriptionValidator))]
    public partial class NewsletterSubscriptionModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }

        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.Fields.Active")]
        public bool Active { get; set; }

        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.Fields.Store")]
        public string StoreName { get; set; }

        [NopResourceDisplayName("Admin.Promotions.NewsletterSubscriptions.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}