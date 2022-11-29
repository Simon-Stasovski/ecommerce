using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider _productProvider;
        public ProductsController(IProductsProvider productsProvider)
        {
            _productProvider = productsProvider;
        }
        [HttpGet]
        public async Task<IActionResult> GetTaskAsync()
        {
            var result = await _productProvider.GetProductsAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Products);
            }

            return NotFound(); 
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await _productProvider.GetProductAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Product);
            }

            return NotFound();
        }
    }
}

