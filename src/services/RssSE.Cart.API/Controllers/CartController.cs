using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RssSE.Cart.API.Data;
using RssSE.Cart.API.Models;
using RssSE.WebApi.Core.Controllers;
using RssSE.WebApi.Core.User.Interfaces;
using System;
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

        [HttpGet("carinho")]
        public async Task<ClientCart> GetCart() => await GetClientCart() ?? new ClientCart(_user.GetUserId());

        [HttpPost("carrinho")]
        public async Task<IActionResult> AddCartItem(CartItem cartItem)
        {
            var clientCart = await GetClientCart();
            if (clientCart is null)
                ManipulateNewCart(cartItem);
            else
                ManipulateExistingCart(clientCart, cartItem);
            if (!IsOperationValid()) return CustomResponse();
            await Commit();
            return CustomResponse();
        }

        private void ManipulateExistingCart(ClientCart cart, CartItem cartItem)
        {
            var itemExistsInCart = cart.ItemExistsInCart(cartItem);
            cart.AddItem(cartItem);
            if (itemExistsInCart)
                _context.CartItems.Update(cart.GetItemByProductId(cartItem.ProductId));
            else
                _context.CartItems.Add(cartItem);
            _context.ClientCarts.Update(cart);
        }

        private void ManipulateNewCart(CartItem cartItem)
        {
            var cart = new ClientCart(_user.GetUserId());
            cart.AddItem(cartItem);
            _context.ClientCarts.Add(cart);
        }

        [HttpPut("carrinho/{productId:guid}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId)
        {
            return CustomResponse();
        }

        [HttpDelete("carrinho/{productId:guid}")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            return CustomResponse();
        }

        private async Task<ClientCart> GetClientCart() =>
            await _context.ClientCarts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.ClientId == _user.GetUserId());

        private async Task Commit()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddProcessError("Não foi possível persistir os dados no banco");
        }
    }
}
