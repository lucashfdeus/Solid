using LHF.Solid.Business.Models;

namespace LHF.Solid.Business.Intefaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
    }
}
