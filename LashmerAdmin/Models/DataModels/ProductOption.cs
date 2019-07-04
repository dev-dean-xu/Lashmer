using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LashmerAdmin.Models.DataModels
{
    [Table("ProductOption")]
    public class ProductOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductOptionId { get; set; }

        public int ProductId { get; set; }
        public string OptionName { get; set; }
        public string OptionValue { get; set; }
        public string OptionDescription { get; set; }

        public virtual Product Product { get; set; }
    }
}
