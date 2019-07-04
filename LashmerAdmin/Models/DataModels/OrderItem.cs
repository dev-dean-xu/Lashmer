using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LashmerAdmin.Models.DataModels
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }
        public string OrderId { get; set; }
        //[Required]
        //public string  ItemName{ get; set; }
        //public string ItemVariant { get; set; }
        public int ProductOptionId { get; set; }
        public string SKU { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        public decimal Price { get; set; }
        public float? Weight { get; set; }
        public string CustomText { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        public virtual ProductOption ProductOption { get; set; }
    }
}