using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Affiliates
{
    public partial class AffiliatedOrderListModel : BaseNopModel
    {
        public AffiliatedOrderListModel()
        {
            AvailablePedidostatuses = new List<SelectListItem>();
            AvailablePaymentStatuses = new List<SelectListItem>();
            AvailableShippingStatuses = new List<SelectListItem>();
        }

        public int AffliateId { get; set; }

        [NopResourceDisplayName("Admin.Affiliates.Pedidos.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [NopResourceDisplayName("Admin.Affiliates.Pedidos.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [NopResourceDisplayName("Admin.Affiliates.Pedidos.Pedidostatus")]
        public int PedidostatusId { get; set; }
        [NopResourceDisplayName("Admin.Affiliates.Pedidos.PaymentStatus")]
        public int PaymentStatusId { get; set; }
        [NopResourceDisplayName("Admin.Affiliates.Pedidos.ShippingStatus")]
        public int ShippingStatusId { get; set; }

        public IList<SelectListItem> AvailablePedidostatuses { get; set; }
        public IList<SelectListItem> AvailablePaymentStatuses { get; set; }
        public IList<SelectListItem> AvailableShippingStatuses { get; set; }
    }
}