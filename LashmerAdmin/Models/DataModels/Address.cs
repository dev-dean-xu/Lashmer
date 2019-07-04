//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace LashmerAdmin.Models.DataModels
//{
//    [Table("Address")]
//    public class Address
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int AddressId { get; set; }
//        [Required]
//        public string Country { get; set; }
//        [Required]
//        public string State { get; set; }
//        [Required]
//        public string City { get; set; }
//        [Required]
//        public string Street { get; set; }
//        [Required]
//        public string ZipCode { get; set; }

//        public virtual Customer Customer { get; set; }
//    }
//}