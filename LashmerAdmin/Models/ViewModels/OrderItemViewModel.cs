using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LashmerAdmin.Models.ViewModels
{
	public class OrderItemViewModel
	{
		public int OrderItemId { get; set; }
		public int Qty { get; set; }
		public decimal Price { get; set; }
		public string ProductName { get; set; }
		public string ProductOptionDescription { get; set; }
	}
}
