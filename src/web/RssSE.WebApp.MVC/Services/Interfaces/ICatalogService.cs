using Refit;
using RssSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssSE.WebApp.MVC.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> GetAll();
        Task<ProductViewModel> Get(Guid id);
    }

    public interface ICatalogServiceRefit
    {
        [Get("/catalogo/produtos")]
        Task<IEnumerable<ProductViewModel>> GetAll();

        [Get("/catalogo/produtos/{id}")]
        Task<ProductViewModel> Get(Guid id);
    }
}
