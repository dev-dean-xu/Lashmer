using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LashmerAdmin.Data;
using LashmerAdmin.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace LashmerAdmin.Models
{
    public class OrderBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Order Build(List<RawOrder> rawOrders)
        {
            if (rawOrders.Count == 0) return null;
            var firstRow = rawOrders.First();

            var order = new Order
            {
                OrderId = firstRow.Order,
                CreatedTime = DateTime.Parse($"{firstRow.Date} {firstRow.Time}"),
                TotalPrice = firstRow.Total,
                Coupon = firstRow.Coupon,
                Currency = firstRow.Currency,
                PaymentMethod = Enum.Parse<PaymentMethod>(firstRow.PaymentMethod.Replace(" ", ""), true),
                PaymentStatus = Enum.Parse<PaymentStatus>(firstRow.Payment.Replace(" ", ""), true),
                Fulfillment = Enum.Parse<Fulfillment>(firstRow.Fulfillment.Replace(" ", ""), true),
                Notes = firstRow.Notes,
                Shipping = firstRow.Shipping,
                Tax = firstRow.Tax,
                Refund = firstRow.Refund,
                TotalAfterRefund = firstRow.TotalAfterRefund,
                DeliveryMethod = firstRow.DeliveryMethod,
                ShippingLabel = firstRow.ShippingLabel,
                Customer = new Customer
                {
                    BillingCustomerName = firstRow.BillingCustomer,
                    BillingCompanyName = firstRow.BillingCompanyName,
                    BillingCountry = firstRow.BillingCountry,
                    BillingCity = firstRow.BillingCity,
                    BillingState = firstRow.BillingState,
                    BillingStreet = firstRow.BillingAddress,
                    BillingZipCode = firstRow.BillingZipCode.Trim('"'),
                    DeliveryCustomerName = firstRow.DeliveryCustomer,
                    DeliveryCompanyName = firstRow.DeliveryCompanyName,
                    DeliveryCountry = firstRow.DeliveryCountry,
                    DeliveryState = firstRow.DeliveryState,
                    DeliveryCity = firstRow.DeliveryCity,
                    DeliveryStreet = firstRow.DeliveryAddress,
                    DeliveryZipCode = firstRow.DeliveryZipCode.Trim('"'),
                    Email = firstRow.Email,
                    PhoneNumber = firstRow.Phone.Trim('"')
                }
            };

            //var products = _dbContext.Products.Include(x => x.Options).ToList();
            _dbContext.Products.Load();
            _dbContext.ProductOptions.Load();
            foreach (var rawOrder in rawOrders)
            {
                order.OrderItems.Add(new OrderItem
                {
                    CustomText = rawOrder.ItemCustomText,
                    Price = rawOrder.ItemPrice,
                    Qty = rawOrder.Qty,
                    SKU = rawOrder.SKU,
                    Weight = rawOrder.ItemWeight,
                    ProductOption = GetOrCreateProductAndProductOption(rawOrder)
                });
            }

            return order;
        }

        private ProductOption GetOrCreateProductAndProductOption(RawOrder rawOrder)
        {
            var product = _dbContext.Products.Local.FirstOrDefault(x =>
                x.ProductName.Equals(rawOrder.ItemName, StringComparison.OrdinalIgnoreCase));
            ProductOption productOption;
            if (product == null)
            {
                product = new Product
                {
                    ProductName = rawOrder.ItemName
                };
                productOption = new ProductOption
                {
                    OptionDescription = rawOrder.ItemVariant,
                    Product = product
                };
                _dbContext.Products.Add(product);
                _dbContext.ProductOptions.Add(productOption);
            }
            else
            {
                productOption = _dbContext.ProductOptions.Local.FirstOrDefault(x =>
                    x.ProductId == product.ProductId &&
                    string.Compare(x.OptionDescription, rawOrder.ItemVariant, StringComparison.OrdinalIgnoreCase) == 0);
                if (productOption == null)
                {
                    productOption = new ProductOption
                    {
                        OptionDescription = rawOrder.ItemVariant,
                        Product = product
                    };
                    _dbContext.ProductOptions.Add(productOption);
                }
            }

            return productOption;
        }
    }
}
