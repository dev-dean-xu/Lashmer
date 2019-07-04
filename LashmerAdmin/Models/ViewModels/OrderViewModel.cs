using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LashmerAdmin.Models.DataModels;

namespace LashmerAdmin.Models.ViewModels
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
        public string Payment { get; set; }
        public string PaymentMethod { get; set; }
        public string Fulfillment { get; set; }
        public decimal Refund { get; set; }
    }
}
