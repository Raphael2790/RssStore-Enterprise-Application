using RssSE.Core.Communication;
using RssSE.WebApp.MVC.Models;
using System;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartViewModel> GetCart();
        Task<ResponseResult> AddItemInCart(ProductItemViewModel productItem);
        Task<ResponseResult> UpdateItemInCart(Guid productId, ProductItemViewModel productItem);
        Task<ResponseResult> RemoveItemInCart(Guid productId);
    }
}
