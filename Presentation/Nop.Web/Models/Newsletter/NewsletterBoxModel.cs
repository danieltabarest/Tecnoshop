using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Boletín informativo
{
    public partial class Boletín informativoBoxModel : BaseNopModel
    {
        public string Boletín informativoEmail { get; set; }
        public bool AllowToUnsubscribe { get; set; }
    }
}