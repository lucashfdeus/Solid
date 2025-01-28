using LHF.Solid.Business.Models;
using LHF.Solid.Business.Services.BadServices;
using Microsoft.AspNetCore.Mvc;

namespace LHF.Solid.Api.Controllers.BadPractice
{
    [ApiController]
    [Route("api/bad/[controller]")]
    public class ProductBadController : ControllerBase
    {
        private readonly ProductBadService _productBadService = new ProductBadService(); // Inicialização direta

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productBadService.GetAllProducts();
            return products.Any() ? Ok(products) : NoContent();
        }

        [HttpPost]
        public IActionResult Add([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _productBadService.AddProduct(product);
                return CreatedAtAction(nameof(GetAll), null);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}