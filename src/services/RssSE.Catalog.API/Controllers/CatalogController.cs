using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RssSE.Catalog.API.Models;
using RssSE.Catalog.API.Models.Repositories;
using RssSE.WebApi.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Catalog.API.Controllers
{
    [Authorize]
    public class CatalogController : MainController
    {
        private readonly IProductRepository _productRepository;
        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [AllowAnonymous]
        [HttpGet("catalogo/produtos")]
        public async Task<IEnumerable<Product>> Index() => await _productRepository.GetAll();

        [HttpGet("catalogo/produto/{id:guid}")]
        public async Task<Product> ProductDetail(Guid id) => await _productRepository.Get(id);

        [HttpGet("catalogo/produtos/lista/{ids}")]
        public async Task<IEnumerable<Product>> GetProductsById(string ids) => await _productRepository.GetProductsById(ids);
    }
}
