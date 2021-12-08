using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Controllers
{
    public class CatalogController : MainController
    {
        private readonly ICatalogService _catalogService;
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            var products = await _catalogService.GetAll(ps, page, q);
            ViewBag.Search = q;
            return View(products);
        }

        [HttpGet("produto-detalhe/{id:guid}")]
        public async Task<IActionResult> ProductDetails(Guid id) => View(await _catalogService.Get(id));
    }
}
