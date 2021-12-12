using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RssSE.Cart.API.Data;
using RssSE.Cart.API.Models;
using RssSE.WebApi.Core.User.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Cart.API.Services.gRPC
{
    [Authorize]
    public class CartGrpcService : PurchaseCart.PurchaseCartBase
    {
        private readonly ILogger<CartGrpcService> _logger;
        private readonly IAspNetUser _aspNetUser;
        private readonly CartDbContext _cartDbContext;

        public CartGrpcService(ILogger<CartGrpcService> logger, IAspNetUser aspNetUser, CartDbContext cartDbContext)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _cartDbContext = cartDbContext;
        }

        public override async Task<CartCustomerResponse> GetCart(GetCartRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Chamando carrinho");
            var userId = _aspNetUser.GetUserId();
            var cart = await GetCustomerCart(userId) ?? new CustomerCart(userId);
            return MapCustomerCartToProtoResponse(cart);
        }

        private async Task<CustomerCart> GetCustomerCart(Guid userId)
        {
            return await _cartDbContext.ClientCarts
                    .Include(x => x.CartItems)
                    .FirstOrDefaultAsync(x => x.ClientId == userId);
        }

        private static CartCustomerResponse MapCustomerCartToProtoResponse(CustomerCart cart)
        {
            var protoCart = new CartCustomerResponse
            {
                Customerid = cart.ClientId.ToString(),
                Discount = (double)cart.Discount,
                Totalvalue = (double)cart.TotalValue,
                Id = cart.Id.ToString(),
                Voucherapplyed = cart.VoucherApplyed
            };

            if(!(cart.Voucher is null))
            {
                protoCart.Voucher = new VoucherResponse
                {
                    Code = cart.Voucher.Code,
                    Discounttype = (int)cart.Voucher.VoucherType,
                    Discountvalue = (double?)cart.Voucher.DiscountValue ?? 0,
                    Percentage = (double?)cart.Voucher.Percentage ?? 0
                };
            }

            if(!(cart.CartItems is null) && cart.CartItems.Any())
            {
                foreach (var item in cart.CartItems)
                {
                    protoCart.Items.Add(
                    new CartItemResponse
                    {
                        Id = item.Id.ToString(),
                        Image = item.Image,
                        Name = item.Name,
                        Productid = item.ProductId.ToString(),
                        Quantity = item.Quantity,
                        Unitvalue = (double)item.UnitValue
                    });
                }
            }

            return protoCart;
        }
    }
}
