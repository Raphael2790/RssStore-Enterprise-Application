using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace RssSE.Identity.API.v1.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        public ICollection<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object obj = null)
        {
            if (IsOperationValid()) return Ok(obj);
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                {"Messages", Errors.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var modelErrors = modelState.Values.SelectMany(x => x.Errors);
            foreach (var error in modelErrors)
                AddProcessError(error.ErrorMessage);
            return CustomResponse();
        }

        protected bool IsOperationValid() => !Errors.Any();
        protected void AddProcessError(string error) => Errors.Add(error);
        protected void ClearProcessErrors() => Errors.Clear();
    }
}
