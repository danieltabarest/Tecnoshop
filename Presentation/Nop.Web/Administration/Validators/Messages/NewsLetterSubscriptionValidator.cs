﻿using FluentValidation;
using Nop.Admin.Models.Messages;
using Nop.Core.Domain.Messages;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Admin.Validators.Messages
{
    public partial class NewsletterSubscriptionValidator : BaseNopValidator<NewsletterSubscriptionModel>
    {
        public NewsletterSubscriptionValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.NewsletterSubscriptions.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));

            SetDatabaseValidationRules<NewsletterSubscription>(dbContext);
        }
    }
}