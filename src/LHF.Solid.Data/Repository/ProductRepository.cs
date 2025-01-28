using LHF.Solid.Business.Intefaces;
using LHF.Solid.Business.Models;
using LHF.Solid.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LHF.Solid.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Set<Product>().ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Set<Product>().AddAsync(product);
            await _context.SaveChangesAsync();
        }
    }
}
