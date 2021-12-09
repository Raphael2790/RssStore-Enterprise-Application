using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Models;

namespace RssSE.WebApp.MVC.Extensions
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPageViewModel pagedModel)
        {
            return View(pagedModel);
        }
    }
}
