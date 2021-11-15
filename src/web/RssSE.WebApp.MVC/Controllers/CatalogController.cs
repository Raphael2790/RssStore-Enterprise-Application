using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Controllers
{
    public class CatalogController : MainController
    {
        private readonly ICatalogServiceRefit _catalogService;
        public CatalogController(ICatalogServiceRefit catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index() => View(await _catalogService.GetAll());

        [HttpGet("produto-detalhe/{id:guid}")]
        public async Task<IActionResult> ProductDetails(Guid id) => View(await _catalogService.Get(id));
    }
}
