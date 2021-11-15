using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssSE.Catalog.API.Models;
using RssSE.Catalog.API.Models.Repositories;
using RssSE.WebApi.Core.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Catalog.API.Controllers
{
    [ApiController]
    [Authorize]
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;
        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [AllowAnonymous]
        [HttpGet("catalogo/produtos")]
        public async Task<IEnumerable<Product>> Index() => await _productRepository.GetAll();

        [ClaimsAuthorize("Catalogo", "Ler")]
        [HttpGet("catalogo/produto/{id:guid}")]
        public async Task<Product> ProductDetail(Guid id) => await _productRepository.Get(id);
    }
}
