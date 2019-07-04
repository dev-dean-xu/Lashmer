using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LashmerAdmin.Models.DataModels
{
    [Table("UserCoupon")]
    public class UserCoupon
    {
        public string UserId { get; set; }
        public string Coupon { get; set; }
    }
}
