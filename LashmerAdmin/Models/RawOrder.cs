using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace LashmerAdmin.Models
{
    public class RawOrder
    {
        [Name("Order #")]
        public string Order { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        [Name("Billing Customer")]
        public string BillingCustomer{get; set; }
        [Name("Billing Company Name")]
        public string BillingCompanyName { get; set; }
        [Name("Billing Country")]
        public string BillingCountry { get; set; }
        [Name("Billing State")]
        public string BillingState { get; set; }
        [Name("Billing City")]
        public string BillingCity { get; set; }
        [Name("Billing Street Name&Number")]
        public string BillingAddress { get; set; }
        [Name("Billing Zip Code")]
        public string BillingZipCode { get; set; }
        [Name("Delivery Customer")]
        public string DeliveryCustomer { get; set; }
        [Name("Delivery Company Name")]
        public string DeliveryCompanyName { get; set; }
        [Name("Delivery Country")]
        public string DeliveryCountry { get; set; }
        [Name("Delivery State")]
        public string DeliveryState { get; set; }
        [Name("Delivery City")]
        public string DeliveryCity { get; set; }
        [Name("Delivery Street Name&Number")]
        public string DeliveryAddress { get; set; }
        [Name("Delivery Zip Code")]
        public string DeliveryZipCode { get; set; }
        [Name("Buyer's Phone #")]
        public string Phone { get; set; }
        [Name("Shipping Label")]
        public string ShippingLabel { get; set; }
        [Name("Buyer's Email")]
        public string Email { get; set; }
        [Name("Delivery Method")]
        public string DeliveryMethod { get; set; }
        [Name("Item's Name")]
        public string ItemName { get; set; }
        [Name("Item's Variant")]
        public string ItemVariant { get; set; }
        public string SKU { get; set; }
        public int Qty { get; set; }
        [Name("Item's Price")]
        public decimal ItemPrice { get; set; }
        [Name("Item's Weight")]
        public float ItemWeight { get; set; }
        [Name("Item's Custom Text")]
        public string ItemCustomText { get; set; }
        public string Coupon { get; set; }
        [Name("Notes to Seller")]
        public string Notes { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string Currency { get; set; }
        [Name("Payment Method")]
        public string PaymentMethod { get; set; }
        public string Payment { get; set; }
        public string Fulfillment { get; set; }
        public decimal Refund { get; set; }
        [Name("Total after refund")]
        public decimal TotalAfterRefund { get; set; }
    }
}
