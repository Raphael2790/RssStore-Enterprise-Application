using RssSE.Bff.Purchases.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services.Interfaces
{
    public interface  ICatalogService
    {
        Task<ItemProductDTO> GetById(Guid productId);
        Task<IEnumerable<ItemProductDTO>> GetItems(IEnumerable<Guid> ids);
    }
}
