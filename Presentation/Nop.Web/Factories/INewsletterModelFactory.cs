using Nop.Web.Models.Boletín informativo;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the interface of the Boletín informativo model factory
    /// </summary>
    public partial interface IBoletín informativoModelFactory
    {
        /// <summary>
        /// Prepare the Boletín informativo box model
        /// </summary>
        /// <returns>Boletín informativo box model</returns>
        Boletín informativoBoxModel PrepareBoletín informativoBoxModel();

        /// <summary>
        /// Prepare the subscription activation model
        /// </summary>
        /// <param name="active">Whether the subscription has been activated</param>
        /// <returns>Subscription activation model</returns>
        SubscriptionActivationModel PrepareSubscriptionActivationModel(bool active);
    }
}
