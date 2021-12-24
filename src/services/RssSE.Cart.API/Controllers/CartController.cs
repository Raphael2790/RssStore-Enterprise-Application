using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RssSE.Cart.API.Data;
using RssSE.Cart.API.Models;
using RssSE.WebApi.Core.Controllers;
using RssSE.WebApi.Core.User.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Cart.API.Controllers
{
    [Authorize]
    public class CartController : MainController
    {
        private readonly IAspNetUser _user;
        private readonly CartDbContext _context;
        public CartController(IAspNetUser user, CartDbContext context)
        {
            _user = user;
            _context = context;
        }

        [HttpGet("carrinho")]
        public async Task<CustomerCart> GetCart() => await GetClientCart() ?? new CustomerCart(_user.GetUserId());

        [HttpPost("carrinho")]
        public async Task<IActionResult> AddItem(CartItem cartItem)
        {
            var clientCart = await GetClientCart();
            if (clientCart is null)
                ManipulateNewCart(cartItem);
            else
                ManipulateExistingCart(clientCart, cartItem);
            if (!IsValidOperation()) return CustomResponse();
            await Commit();
            return CustomResponse();
        }

        [HttpPut("carrinho/{productId:guid}")]
        public async Task<IActionResult> UpdateItem(Guid productId, CartItem item)
        {
            var cart = await GetClientCart();
            var cartItem = await GetValidCartItem(productId, cart, item);
            if (cartItem is null) return CustomResponse();
            cart.UpdateQuantity(cartItem, item.Quantity);
            ValidateCart(cart);
            if (!IsValidOperation()) return CustomResponse();
            _context.CartItems.Update(cartItem);
            _context.ClientCarts.Update(cart);
            await Commit();
            return CustomResponse();
        }

        [HttpDelete("carrinho/{productId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var cart = await GetClientCart();
            var cartItem = await GetValidCartItem(productId,cart);
            if (cartItem is null) return CustomResponse();
            ValidateCart(cart);
            if (!IsValidOperation()) return CustomResponse();
            cart.RemoveItem(cartItem);
            _context.CartItems.Remove(cartItem);
            _context.ClientCarts.Update(cart);
            await Commit();
            return CustomResponse();
        }

        private async Task<CustomerCart> GetClientCart() =>
            await _context.ClientCarts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());

        [HttpPost("carrinho/aplicar-voucher")]
        public async Task<IActionResult> ApplyVoucher(Voucher voucher)
        {
            var cart = await GetCart();
            cart.ApplyVoucher(voucher);
            _context.ClientCarts.Update(cart);
            await Commit();
            return CustomResponse();
        }

        private async Task Commit()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddProcessError("Não foi possível persistir os dados no banco");
        }

        private void ManipulateExistingCart(CustomerCart cart, CartItem cartItem)
        {
            var itemExistsInCart = cart.ItemExistsInCart(cartItem);
            cart.AddItem(cartItem);
            ValidateCart(cart);
            if (itemExistsInCart)
                _context.CartItems.Update(cart.GetItemByProductId(cartItem.ProductId));
            else
                _context.CartItems.Add(cartItem);
            _context.ClientCarts.Update(cart);
        }

        private void ManipulateNewCart(CartItem cartItem)
        {
            var cart = new CustomerCart(_user.GetUserId());
            cart.AddItem(cartItem);
            ValidateCart(cart);
            _context.ClientCarts.Add(cart);
        }

        private async Task<CartItem> GetValidCartItem(Guid productId, CustomerCart cart, CartItem item = null)
        {
            if(item != null && productId != item.ProductId)
            {
                AddProcessError("O item não corresponde ao informado!");
                return null;
            }

            if(cart is null)
            {
                AddProcessError("Carrinho não encontrado!");
                return null;
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.CartId == cart.Id);

            if(cartItem is null || !cart.ItemExistsInCart(cartItem))
            {
                AddProcessError("O item não está no carrinho!");
                return null;
            }

            return cartItem;
        }

        private bool ValidateCart(CustomerCart cart)
        {
            if (cart.IsValid()) return true;
            cart.ValidationResult.Errors.ToList().ForEach(e => AddProcessError(e.ErrorMessage));
            return false;
        }
    }
}
