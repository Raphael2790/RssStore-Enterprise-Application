using RssSE.Bff.Purchases.Models;
using System;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services.Interfaces
{
    public interface  ICatalogService
    {
        Task<ItemProductDTO> GetById(Guid productId);
    }
}
