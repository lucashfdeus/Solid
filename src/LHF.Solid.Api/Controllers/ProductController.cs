using LHF.Solid.Business.Intefaces;
using LHF.Solid.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace LHF.Solid.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : MainController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return products.Any() ? Ok(products) : NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetAll), null);
        }
    }
}
