using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LashmerAdmin.Models.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(256, ErrorMessage ="The length of your name should be less than 256 characters.")]
        public string FullName { get; set; }
    }
}
