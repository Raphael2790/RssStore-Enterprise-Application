using Microsoft.EntityFrameworkCore;
using RssSE.Catalog.API.Models;
using RssSE.Catalog.API.Models.Repositories;
using RssSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Catalog.API.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _context;
        public ProductRepository(CatalogContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Product product) => _context.Products.Add(product);

        public void Dispose() => _context?.Dispose();

        public async Task<Product> Get(Guid id) =>
            await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Product>> GetAll() =>
            await _context.Products.AsNoTracking().ToListAsync();

        public void Update(Product product) => _context.Products.Update(product);
    }
}
