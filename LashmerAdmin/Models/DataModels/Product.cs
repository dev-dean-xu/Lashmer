using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LashmerAdmin.Models.DataModels
{
  [Table("Product")]
  public class Product
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }
    [Required]
    public string ProductName { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public virtual ICollection<ProductOption> Options { get; set; } = new HashSet<ProductOption>();
  }
}
