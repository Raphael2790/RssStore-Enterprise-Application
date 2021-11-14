using RssSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Catalog.API.Models.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> Get(Guid id);
        void Add(Product product);
        void Update(Product product);
    }
}
