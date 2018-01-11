using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Pedidos
{
    public partial class OrderAverageReportLineSummaryModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.SalesReport.Average.Pedidostatus")]
        public string Pedidostatus { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Average.SumTodayPedidos")]
        public string SumTodayPedidos { get; set; }
        
        [NopResourceDisplayName("Admin.SalesReport.Average.SumThisWeekPedidos")]
        public string SumThisWeekPedidos { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Average.SumThisMonthPedidos")]
        public string SumThisMonthPedidos { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Average.SumThisYearPedidos")]
        public string SumThisYearPedidos { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Average.SumAllTimePedidos")]
        public string SumAllTimePedidos { get; set; }
    }
}
