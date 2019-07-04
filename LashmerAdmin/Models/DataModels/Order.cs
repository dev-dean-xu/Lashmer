using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LashmerAdmin.Models.DataModels
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public string OrderId { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CustomerId { get; set; }
        public string Coupon { get; set; }
        public string Notes { get; set; }
        [Required]
        public decimal Shipping { get; set; }
        [Required]
        public string DeliveryMethod { get; set; }
        public string ShippingLabel { get; set; }
        [Required]
        public decimal Tax { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public decimal Refund { get; set; }
        [Required]
        public decimal TotalAfterRefund { get; set; }
        public string Currency { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public PaymentStatus PaymentStatus { get; set; }
        [Required]
        public Fulfillment Fulfillment { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public virtual Customer Customer{get;set;}
    }

    public enum Fulfillment
    {
        NotFulfilled,
        Fulfilled
    }

    public enum PaymentStatus
    {
        NotPaid,
        Paid
    }

    public enum PaymentMethod
    {
        PayPal,
        Stripe,
        Square,
        Cash
    }
}
