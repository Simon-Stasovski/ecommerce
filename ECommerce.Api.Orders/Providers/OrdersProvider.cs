using AutoMapper;
using System;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        readonly ILogger logger;
        readonly IMapper mapper;
        readonly OrdersDbContext dbContext;

        public OrdersProvider(ILogger<OrdersProvider> logger, IMapper mapper, OrdersDbContext dbContext)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.dbContext = dbContext;

            SeedData();
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrderAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
                if (orders != null)
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);

                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, "Not found");
            }
        }
        public void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order() { Id = 1, CustomerId = 1, OrderDate = new DateTime(2022, 11, 16), Total = 100.78m, Items = new List<Db.OrderItem> { new Db.OrderItem() { Id = 1, ProductId = 1, Quantity = 3, UnitPrice = 100 }, new Db.OrderItem() { Id = 5, ProductId = 2, Quantity = 5, UnitPrice = 9.99m }, new Db.OrderItem() { Id = 8, ProductId = 3, Quantity = 6, UnitPrice = 12 } } });
                dbContext.Orders.Add(new Db.Order() { Id = 2, CustomerId = 1, OrderDate = new DateTime(2022, 11, 15), Total = 40.48m, Items = new List<Db.OrderItem> { new Db.OrderItem() { Id = 2, ProductId = 1, Quantity = 3, UnitPrice = 100 }, new Db.OrderItem() { Id = 6, ProductId = 2, Quantity = 5, UnitPrice = 9.99m } } });
                dbContext.Orders.Add(new Db.Order() { Id = 3, CustomerId = 2, OrderDate = new DateTime(2022, 11, 14), Total = 134.23m, Items = new List<Db.OrderItem> { new Db.OrderItem() { Id = 3, ProductId = 1, Quantity = 3, UnitPrice = 100 } } });
                dbContext.Orders.Add(new Db.Order() { Id = 4,  CustomerId = 3, OrderDate = new DateTime(2022, 11, 12), Total = 22.12m, Items = new List<Db.OrderItem> { new Db.OrderItem() { Id = 4, ProductId = 2, Quantity = 5, UnitPrice = 9.99m }, new Db.OrderItem() { Id = 7, ProductId = 3, Quantity = 6, UnitPrice = 12 } } });
                dbContext.SaveChanges();
            }
        }
    }
}
