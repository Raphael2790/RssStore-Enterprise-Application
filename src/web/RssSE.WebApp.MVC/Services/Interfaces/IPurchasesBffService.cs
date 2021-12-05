using RssSE.Core.Communication;
using RssSE.WebApp.MVC.Models;
using System;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services.Interfaces
{
    public interface IPurchasesBffService
    {
        Task<CartViewModel> GetCart();
        Task<int> GetCartItemsAmount();
        Task<ResponseResult> AddItemInCart(CartItemViewModel productItem);
        Task<ResponseResult> UpdateItemInCart(Guid productId, CartItemViewModel productItem);
        Task<ResponseResult> RemoveItemInCart(Guid productId);
        Task<ResponseResult> ApplyVoucherCart(string voucher);
        OrderTransactionViewModel MapToOrder(CartViewModel cart, AddressViewModel address);
    }
}
