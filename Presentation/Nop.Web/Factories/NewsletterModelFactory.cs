using Nop.Core.Domain.Customers;
using Nop.Services.Localization;
using Nop.Web.Models.Boletín informativo;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the Boletín informativo model factory
    /// </summary>
    public partial class Boletín informativoModelFactory : IBoletín informativoModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly CustomerSettings _customerSettings;

        #endregion

        #region Ctor

        public Boletín informativoModelFactory(ILocalizationService localizationService,
            CustomerSettings customerSettings)
        {
            this._localizationService = localizationService;
            this._customerSettings = customerSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the Boletín informativo box model
        /// </summary>
        /// <returns>Boletín informativo box model</returns>
        public virtual Boletín informativoBoxModel PrepareBoletín informativoBoxModel()
        {
            var model = new Boletín informativoBoxModel()
            {
                AllowToUnsubscribe = _customerSettings.Boletín informativoBlockAllowToUnsubscribe
            };
            return model;
        }

        /// <summary>
        /// Prepare the subscription activation model
        /// </summary>
        /// <param name="active">Whether the subscription has been activated</param>
        /// <returns>Subscription activation model</returns>
        public virtual SubscriptionActivationModel PrepareSubscriptionActivationModel(bool active)
        {
            var model = new SubscriptionActivationModel();
            model.Result = active
                ? _localizationService.GetResource("Boletín informativo.ResultActivated")
                : _localizationService.GetResource("Boletín informativo.ResultDeactivated");

            return model;
        }

        #endregion
    }
}
