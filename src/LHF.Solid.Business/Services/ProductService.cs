using LHF.Solid.Business.Intefaces;
using LHF.Solid.Business.Models;

namespace LHF.Solid.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("O nome do produto é obrigatório.");
            }

            await _productRepository.AddAsync(product);
        }
    }
}
