using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using LashmerAdmin.Data;
using LashmerAdmin.Models;
using LashmerAdmin.Models.DataModels;
using LashmerAdmin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LashmerAdmin.Controllers
{
	[Authorize(Policy = "ApiUser")]
	[Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
	    private readonly ILogger _logger;
	    private readonly ApplicationDbContext _dbContext;

	    public OrdersController(ILogger logger, ApplicationDbContext dbContext)
	    {
		    _logger = logger;
		    _dbContext = dbContext;
	    }

		// GET: api/Orders
		[HttpGet]
        public IEnumerable<OrderViewModel> Get()
		{
			var orders = _dbContext.Orders.Include(x => x.OrderItems).ThenInclude(o => o.ProductOption)
				.ThenInclude(p => p.Product).Include(x => x.Customer).Select(x => new OrderViewModel
				{
					OrderId = x.OrderId,
					CustomerName = x.Customer.BillingCustomerName,
					Email = x.Customer.Email,
					PhoneNumber = x.Customer.PhoneNumber,
					Fulfillment = x.Fulfillment.ToString(),
					Payment = x.PaymentStatus.ToString(),
					PaymentMethod = x.PaymentMethod.ToString(),
					Refund = x.Refund,
					Shipping = x.Shipping,
					Total = x.TotalPrice,
					Items = x.OrderItems.Select(item => new OrderItemViewModel
					{
						OrderItemId = item.OrderItemId,
						Price = item.Price,
						Qty = item.Qty,
						ProductName = item.ProductOption.Product.ProductName,
						ProductOptionDescription = item.ProductOption.OptionDescription
					}).ToList()
				});

			return orders;
		}

	    // GET: api/Orders/10001
	    [HttpGet("{id}", Name = "Get")]
	    public Order Get(string id)
	    {
			var order = _dbContext.Orders.Include(x => x.OrderItems).ThenInclude(o => o.ProductOption)
				.ThenInclude(p => p.Product).Include(x => x.Customer)
				.FirstOrDefault(x => x.OrderId == id);

			return order;
		}

		[Route("upload")]
	    [HttpPost]
	    public async Task<IActionResult> UploadOrders(IFormFile file)
	    {
		    _logger.Debug("Starting to upload orders");

		    try
		    {
			    await SaveOrdersToTableAsync(file).ConfigureAwait(false);
		    }
		    catch (Exception ex)
		    {
			    return BadRequest(ex.Message);
		    }

		    return Ok();
	    }

		private async Task SaveOrdersToTableAsync(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				throw new Exception("The file you uploaded is empty.");
			}

			var errorBuilder = new StringBuilder();

			using (var reader = new StreamReader(file.OpenReadStream()))
			using (var csv = new CsvReader(reader))
			{
				var rowCount = 1;
				var rawOrders = csv.GetRecords<RawOrder>().ToList();
				if (!rawOrders.Any())
				{
					throw new Exception("Cannot read any orders from the file.");
				}

				var orderGroups = rawOrders.GroupBy(x => x.Order);
				foreach (var rawOrder in orderGroups)
				{
					try
					{
						var newOrder = new OrderBuilder(_dbContext).Build(rawOrder.ToList());
						var order = _dbContext.Orders.Include(x => x.OrderItems).Include(x => x.Customer).FirstOrDefault(x => x.OrderId == rawOrder.Key);
						if (order != null)
						{
							// Delete the existing order
							_dbContext.OrderItems.RemoveRange(order.OrderItems);
							_dbContext.Orders.Remove(order);
							_dbContext.Customers.Remove(order.Customer);
							_dbContext.SaveChanges();
						}

						// Insert new order                                
						await _dbContext.Orders.AddAsync(newOrder).ConfigureAwait(false);
						await _dbContext.SaveChangesAsync().ConfigureAwait(false);
					}
					catch (Exception ex1)
					{
						errorBuilder.AppendLine($"<br> Row {rowCount}: {ex1.Message} <br/>");
					}
					rowCount++;
				}
			}

			_logger.Information("Orders have uploaded with {errorCount} error(s).", errorBuilder.Length);
			if (errorBuilder.Length > 0) throw new Exception(errorBuilder.ToString());
		}

        //// POST: api/Orders
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Orders/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
