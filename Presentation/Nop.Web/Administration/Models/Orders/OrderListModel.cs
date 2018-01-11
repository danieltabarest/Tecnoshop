using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Pedidos
{
    public partial class OrderListModel : BaseNopModel
    {
        public OrderListModel()
        {
            PedidostatusIds = new List<int>();
            PaymentStatusIds = new List<int>();
            ShippingStatusIds = new List<int>();
            AvailablePedidostatuses = new List<SelectListItem>();
            AvailablePaymentStatuses = new List<SelectListItem>();
            AvailableShippingStatuses = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
            AvailablePaymentMethods = new List<SelectListItem>();
            AvailableCountries = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Pedidos.List.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.Pedidostatus")]
        [UIHint("MultiSelect")]
        public List<int> PedidostatusIds { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.PaymentStatus")]
        [UIHint("MultiSelect")]
        public List<int> PaymentStatusIds { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.ShippingStatus")]
        [UIHint("MultiSelect")]
        public List<int> ShippingStatusIds { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.PaymentMethod")]
        public string PaymentMethodSystemName { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.Store")]
        public int StoreId { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.Vendor")]
        public int VendorId { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.Warehouse")]
        public int WarehouseId { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.Product")]
        public int ProductId { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.BillingEmail")]
        [AllowHtml]
        public string BillingEmail { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.BillingLastName")]
        [AllowHtml]
        public string BillingLastName { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.BillingCountry")]
        public int BillingCountryId { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.OrderNotes")]
        [AllowHtml]
        public string OrderNotes { get; set; }

        [NopResourceDisplayName("Admin.Pedidos.List.GoDirectlyToNumber")]
        public string GoDirectlyToCustomOrderNumber { get; set; }

        public bool IsLoggedInAsVendor { get; set; }


        public IList<SelectListItem> AvailablePedidostatuses { get; set; }
        public IList<SelectListItem> AvailablePaymentStatuses { get; set; }
        public IList<SelectListItem> AvailableShippingStatuses { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableVendors { get; set; }
        public IList<SelectListItem> AvailableWarehouses { get; set; }
        public IList<SelectListItem> AvailablePaymentMethods { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }
    }
}