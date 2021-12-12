using RssSE.Bff.Purchases.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Services.gRPC
{
    public interface ICartGrpcService
    {
        Task<CartDTO> GetCart();
    }

    public class CartGrpcService : ICartGrpcService
    {
        private readonly PurchaseCart.PurchaseCartClient _purchaseCartClient;

        public CartGrpcService(PurchaseCart.PurchaseCartClient purchaseCartClient)
        {
            _purchaseCartClient = purchaseCartClient;
        }

        public async Task<CartDTO> GetCart()
        {
            var response = await _purchaseCartClient.GetCartAsync(new GetCartRequest());
            return MapProtoResponseToOrderDTO(response);
        }

        private CartDTO MapProtoResponseToOrderDTO(CartCustomerResponse cartResponse) 
        {
            var cart = new CartDTO
            {
                Discount = (decimal)cartResponse.Discount,
                TotalValue = (decimal)cartResponse.Totalvalue,
                VoucherApplyed = cartResponse.Voucherapplyed,
                CartItems = new List<CartItemDTO>()
            };

            if(!(cartResponse.Voucher is null))
            {
                cart.Voucher = new VoucherDTO
                {
                    Code = cartResponse.Voucher.Code,
                    DiscountValue = (decimal?)cartResponse.Voucher.Discountvalue,
                    Percentage = (decimal?)cartResponse.Voucher.Percentage,
                    DiscountType = cartResponse.Voucher.Discounttype
                };
            }

            if(!(cartResponse.Items is null) && cartResponse.Items.Any())
            {
                foreach (var item in cartResponse.Items)
                {
                    cart.CartItems.Add(new CartItemDTO
                    {
                        Image = item.Image,
                        Name = item.Name,
                        Quantity = item.Quantity,
                        ProductId = Guid.Parse(item.Productid),
                        UnitValue = (decimal)item.Unitvalue
                    });
                }
            }

            return cart;
        }
    }
}
