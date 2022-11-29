using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        readonly ILogger _logger;
        readonly IMapper _mapper;
        readonly ProductsDbContext _dbContext;
        public async Task<(bool IsSuccess, IEnumerable<Models.Product> Products, 
            string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await _dbContext.Products.ToListAsync();
                if(products != null && products.Any())
                {
                    var result = _mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);

                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, "Not found");
            }
        }
        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;   
            _mapper = mapper;
            SeedData();
        }
        private void SeedData()
        {
            if (!_dbContext.Products.Any())
            {
                _dbContext.Products.Add(new Db.Product() { Id = 1, Name = "Keyboard", Price = 119.99M, Inventory = 8 });
                _dbContext.Products.Add(new Db.Product() { Id = 2, Name = "Mouse", Price = 49.99M, Inventory = 9 });
                _dbContext.Products.Add(new Db.Product() { Id = 3, Name = "Cheese", Price = 1.99M, Inventory = 13 });
                _dbContext.Products.Add(new Db.Product() { Id = 4, Name = "Book", Price = 13.99M, Inventory = 21 });
                _dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, Models.Product Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var products = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (products != null)
                {
                    var result = _mapper.Map<Db.Product, Models.Product>(products);

                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, "Not found");
            }
        }
    }
}
