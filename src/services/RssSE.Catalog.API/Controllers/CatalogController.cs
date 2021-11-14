using Microsoft.AspNetCore.Mvc;
using RssSE.Catalog.API.Models;
using RssSE.Catalog.API.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.Catalog.API.Controllers
{
    [ApiController]
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;
        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("catalogo/produtos")]
        public async Task<IEnumerable<Product>> Index() => await _productRepository.GetAll();

        [HttpGet("catalogo/produto/{id:guid}")]
        public async Task<Product> ProductDetail(Guid id) => await _productRepository.Get(id);
    }
}
