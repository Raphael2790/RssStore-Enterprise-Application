using Microsoft.AspNetCore.Mvc;
using RssSE.WebApp.MVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace RssSE.WebApp.MVC.Controllers
{
    public abstract class MainController : Controller
    {
        protected bool ResponseHasErrors(ResponseResult response)
        {
            if (response != null && response.Errors.Messages.Any())
            {
                AddErrorsInModelState(response.Errors.Messages);
                return true;
            }

            return false;
        }

        private void AddErrorsInModelState(IEnumerable<string> messages)
        {
            foreach (var message in messages)
                ModelState.AddModelError(string.Empty, message);
        }

        protected void AddValidationError(string message) => ModelState.AddModelError(string.Empty, message);

        protected bool IsValidOperation() => ModelState.ErrorCount == 0;
    }
}
