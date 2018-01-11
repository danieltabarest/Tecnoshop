namespace Nop.Core.Domain.Pedidos
{
    /// <summary>
    /// Represents an order average report line summary
    /// </summary>
    public partial class OrderAverageReportLineSummary
    {
        /// <summary>
        /// Gets or sets the order status
        /// </summary>
        public Pedidostatus Pedidostatus { get; set; }

        /// <summary>
        /// Gets or sets the sum today total
        /// </summary>
        public decimal SumTodayPedidos { get; set; }

        /// <summary>
        /// Gets or sets the today count
        /// </summary>
        public int CountTodayPedidos { get; set; }

        /// <summary>
        /// Gets or sets the sum this week total
        /// </summary>
        public decimal SumThisWeekPedidos { get; set; }

        /// <summary>
        /// Gets or sets the this week count
        /// </summary>
        public int CountThisWeekPedidos { get; set; }

        /// <summary>
        /// Gets or sets the sum this month total
        /// </summary>
        public decimal SumThisMonthPedidos { get; set; }

        /// <summary>
        /// Gets or sets the this month count
        /// </summary>
        public int CountThisMonthPedidos { get; set; }

        /// <summary>
        /// Gets or sets the sum this year total
        /// </summary>
        public decimal SumThisYearPedidos { get; set; }

        /// <summary>
        /// Gets or sets the this year count
        /// </summary>
        public int CountThisYearPedidos { get; set; }

        /// <summary>
        /// Gets or sets the sum all time total
        /// </summary>
        public decimal SumAllTimePedidos { get; set; }

        /// <summary>
        /// Gets or sets the all time count
        /// </summary>
        public int CountAllTimePedidos { get; set; }
    }
}
