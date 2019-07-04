using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using LashmerAdmin.Data;
using LashmerAdmin.Models;
using LashmerAdmin.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LashmerAdmin.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;
        public OrderController(ILogger logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessages"];
          return View(_dbContext.Orders.Include(x => x.OrderItems).ThenInclude(o => o.ProductOption)
            .ThenInclude(p => p.Product).Include(x => x.Customer).ToList());
        }

        public IActionResult Details(string id)
        {
          var order = _dbContext.Orders.Include(x => x.OrderItems).ThenInclude(o => o.ProductOption)
            .ThenInclude(p => p.Product).Include(x => x.Customer)
            .FirstOrDefault(x => x.OrderId == id);
            return View(order);
        }

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
                TempData["ErrorMessages"] = ex.Message;
            }

            // full path to file in temp location
            //var filePath = Path.GetTempFileName();
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream).ConfigureAwait(false);
            //}
            
            return RedirectToAction("Index");
        }

        private void LogError(string errorMessage)
        {
            TempData["ErrorMessages"] = errorMessage;
            _logger.Error(errorMessage);
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
                        var order = _dbContext.Orders.Include(x=>x.OrderItems).Include(x=>x.Customer).FirstOrDefault(x => x.OrderId == rawOrder.Key);
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
            if (errorBuilder.Length > 0) throw new Exception(errorBuilder.ToString());
        }
    }
}