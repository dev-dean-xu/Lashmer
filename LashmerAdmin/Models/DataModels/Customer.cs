using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LashmerAdmin.Models.DataModels
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int CustomerId { get; set; }
        public string BillingCustomerName { get; set;}
        public string BillingCompanyName { get; set; }
        public string BillingCountry { get; set; }
        public string BillingState { get; set; }
        public string BillingCity { get; set; }
        public string BillingStreet { get; set; }
        public string BillingZipCode { get; set; }

        [Required]
        public string DeliveryCustomerName { get; set; }
        public string DeliveryCompanyName { get; set; }
        public string DeliveryCountry { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryStreet { get; set; }
        public string DeliveryZipCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }

        public bool Equals(Customer obj)
        {
            return base.Equals(obj);
        }
    }
}