using RssSE.Catalog.API.Extensions;
using RssSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Catalog.API.Models.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedResult<Product>> GetAll(int pageSize, int pageIndex, string query = null);
        Task<Product> Get(Guid id);
        Task<IEnumerable<Product>> GetProductsById(string ids);
        void Add(Product product);
        void Update(Product product);
    }
}
