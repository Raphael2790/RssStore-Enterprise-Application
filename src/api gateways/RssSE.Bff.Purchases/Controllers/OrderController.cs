using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssSE.Bff.Purchases.Models;
using RssSE.Bff.Purchases.Services.Interfaces;
using RssSE.WebApi.Core.Controllers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Bff.Purchases.Controllers
{
    [Authorize]
    public class OrderController : MainController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICustomerService _customerService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public OrderController(ICatalogService catalogService, 
                                ICustomerService customerService, 
                                ICartService cartService, 
                                IOrderService orderService)
        {
            _catalogService = catalogService;
            _customerService = customerService;
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpPost("compras/pedido")]
        public async Task<IActionResult> AddOrder(OrderDTO order)
        {
            var cart = await _cartService.GetCart();
            var products = await _catalogService.GetItems(cart.CartItems.Select(p => p.ProductId));
            var address = await _customerService.GetAddress();

            if (!await ValidateCartProducts(cart, products)) return CustomResponse();

            PopulateOrderInfo(cart, address, order);
            return CustomResponse(await _orderService.FinishOrder(order));
        }

        [HttpGet("compras/pedido/ultimo")]
        public async Task<IActionResult> GetLastOrder()
        {
            var order = await _orderService.GetLastOrder();
            if(order is null)
            {
                AddProcessError("Pedido não encontrado");
                return CustomResponse();
            }
            return CustomResponse(order);
        }

        [HttpGet("compras/pedido/lista-cliente")]
        public async Task<IActionResult> ListByCustomer()
        {
            var orders = await _orderService.GetListByCustomerId();
            return orders is null || !orders.Any() ? NotFound() : CustomResponse(orders);
        }

        private async Task<bool> ValidateCartProducts(CartDTO cart, IEnumerable<ItemProductDTO> products)
        {
            if (cart.CartItems.Count != products.Count())
            {
                var unavailableItens = cart.CartItems.Select(c => c.ProductId)
                    .Except(products.Select(x => x.Id)).ToList();

                foreach (var itemId in unavailableItens)
                {
                    var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == itemId);
                    AddProcessError($"O item {cartItem.Name} não está mais disponível no catálogo, o remova do carrinho para prosseguir com a compra");
                }
                return false;
            }

            foreach (var item in cart.CartItems)
            {
                var catalogProduct = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (catalogProduct.Value != item.UnitValue)
                {
                    var msgError = $"O produto {item.Name} mudou de valor (de: " +
                                 $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.UnitValue)} para: " +
                                 $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", catalogProduct.Value)}) desde que foi adicionado ao carrinho.";
                    AddProcessError(msgError);

                    var responseRemove = await _cartService.RemoveItemInCart(item.ProductId);
                    if (ResponseHasErrors(responseRemove))
                    {
                        AddProcessError($"Não foi possível remover automaticamente o produto {item.Name} do seu carrinho, _" +
                                                   "remova e adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    item.UnitValue = catalogProduct.Value;
                    var responseAdd = await _cartService.AddItemInCart(item);
                    if (ResponseHasErrors(responseAdd))
                    {
                        AddProcessError($"Não foi possível atualizar automaticamente o produto {item.Name} do seu carrinho, _" +
                                                   "adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    ClearProcessErrors();
                    AddProcessError(msgError + " Atualizamos o valor em seu carrinho, realize a conferência do pedido e se preferir remova o produto");

                    return false;
                }
            }

            return true;
        }

        private void PopulateOrderInfo(CartDTO cart, AddressDTO address, OrderDTO order)
        {
            order.VoucherCode = cart.Voucher?.Code;
            order.VoucherApplyed = cart.VoucherApplyed;
            order.TotalValue = cart.TotalValue;
            order.Discount = cart.Discount;

            foreach (var item in cart.CartItems)
            {
                order.OrderItems.Add(new OrderItemDTO
                {
                    Image = item.Image,
                    ProductId = item.ProductId,
                    ProductName = item.Name,
                    Quantity = item.Quantity,
                    UnitValue = item.UnitValue
                });
            }

            order.Address = address;
        }
    }
}
