using System;
using System.Collections.Generic;
using Nop.Core.Domain.Pedidos;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Order
{
    public partial class CustomerOrderListModel : BaseNopModel
    {
        public CustomerOrderListModel()
        {
            Pedidos = new List<OrderDetailsModel>();
            RecurringPedidos = new List<RecurringOrderModel>();
            RecurringPaymentErrors = new List<string>();
        }

        public IList<OrderDetailsModel> Pedidos { get; set; }
        public IList<RecurringOrderModel> RecurringPedidos { get; set; }
        public IList<string> RecurringPaymentErrors { get; set; }


        #region Nested classes

        public partial class OrderDetailsModel : BaseNopEntityModel
        {
            public string CustomOrderNumber { get; set; }
            public string OrderTotal { get; set; }
            public bool IsReturnRequestAllowed { get; set; }
            public Pedidostatus PedidostatusEnum { get; set; }
            public string Pedidostatus { get; set; }
            public string PaymentStatus { get; set; }
            public string ShippingStatus { get; set; }
            public DateTime CreatedOn { get; set; }
        }

        public partial class RecurringOrderModel : BaseNopEntityModel
        {
            public string StartDate { get; set; }
            public string CycleInfo { get; set; }
            public string NextPayment { get; set; }
            public int TotalCycles { get; set; }
            public int CyclesRemaining { get; set; }
            public int InitialOrderId { get; set; }
            public bool CanRetryLastPayment { get; set; }
            public string InitialOrderNumber { get; set; }
            public bool CanCancel { get; set; }
        }

        #endregion
    }
}