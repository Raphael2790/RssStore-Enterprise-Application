using Dapper;
using Microsoft.EntityFrameworkCore;
using RssSE.Catalog.API.Extensions;
using RssSE.Catalog.API.Models;
using RssSE.Catalog.API.Models.Repositories;
using RssSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<PagedResult<Product>> GetAll(int pageSize, int pageIndex, string query = null) 
        {
            string sql = @$"SELECT * FROM Products
                         WHERE (@Name IS NULL OR Name LIKE '%' + @Name + '%')
                         ORDER BY [Name]
                         OFFSET {pageSize * (pageIndex - 1)} ROWS
                         FETCH NEXT {pageSize} ROWS ONLY
                         SELECT COUNT(Id) FROM Products
                         WHERE (@Name IS NULL OR Name LIKE '%' + @Name + '%')";

            var multi = await _context.Database.GetDbConnection().QueryMultipleAsync(sql, new { Name = query});
            var products = multi.Read<Product>();
            var total = multi.Read<int>().FirstOrDefault();

            return new PagedResult<Product>
            {
                List = products,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query, 
                TotalResults = total
            };
        }

        public async Task<IEnumerable<Product>> GetProductsById(string ids)
        {
            var idsGuid = ids.Split(',')
                 .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) return new List<Product>();

            var idsValue = idsGuid.Select(id => id.Value);

            return await _context.Products.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Active).ToListAsync();
        }

        public void Update(Product product) => _context.Products.Update(product);
    }
}
