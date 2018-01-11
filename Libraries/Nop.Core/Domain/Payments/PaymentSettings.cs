using System.Collections.Generic;
using Nop.Core.Configuration;

namespace Nop.Core.Domain.Payments
{
    public class PaymentSettings : ISettings
    {
        public PaymentSettings()
        {
            ActivePaymentMethodSystemNames = new List<string>();
        }

        /// <summary>
        /// Gets or sets a system names of active Formas de pagos
        /// </summary>
        public List<string> ActivePaymentMethodSystemNames { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to repost (complete) payments for redirection Formas de pagos
        /// </summary>
        public bool AllowRePostingPayments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should bypass 'select Formas de pago' page if we have only one Formas de pago
        /// </summary>
        public bool BypassPaymentMethodSelectionIfOnlyOne { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show Formas de pago descriptions on "choose Formas de pago" checkout page in the public store
        /// </summary>
        public bool ShowPaymentMethodDescriptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should skip 'payment info' page for redirection Formas de pagos
        /// </summary>
        public bool SkipPaymentInfoStepForRedirectionPaymentMethods { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to cancel the recurring payment after failed last payment 
        /// </summary>
        public bool CancelRecurringPaymentsAfterFailedPayment { get; set; }
    }
}