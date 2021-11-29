using RssSE.Bff.Purchases.Models;
using RssSE.Core.Communication;
using System;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO> GetCart();
        Task<ResponseResult> AddItemInCart(CartItemDTO cartItem);
        Task<ResponseResult> UpdateItemInCart(Guid productId, CartItemDTO cartItem);
        Task<ResponseResult> RemoveItemInCart(Guid productId);
        Task<ResponseResult> ApplyVoucherOnCart(VoucherDTO voucher);
    }
}
