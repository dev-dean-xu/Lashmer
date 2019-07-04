using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LashmerAdmin.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }
        [Display(Description = "Account Name")]
        public string UserName { get; set; }
        [Display(Description = "Name")]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Coupons { get; set; }
    }
}
