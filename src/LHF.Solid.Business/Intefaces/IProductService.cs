using LHF.Solid.Business.Models;

namespace LHF.Solid.Business.Intefaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
    }
}
