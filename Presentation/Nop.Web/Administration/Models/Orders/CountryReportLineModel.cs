using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Pedidos
{
    public partial class CountryReportLineModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.SalesReport.Country.Fields.CountryName")]
        public string CountryName { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Country.Fields.TotalPedidos")]
        public int TotalPedidos { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Country.Fields.SumPedidos")]
        public string SumPedidos { get; set; }
    }
}