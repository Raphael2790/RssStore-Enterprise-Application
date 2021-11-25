using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RssSE.Core.Communication;
using System.Collections.Generic;
using System.Linq;

namespace RssSE.WebApi.Core.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        public ICollection<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object obj = null)
        {
            if (IsValidOperation()) return Ok(obj);
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

        protected ActionResult CustomResponse(ValidationResult validation)
        {
            foreach (var error in validation.Errors)
                AddProcessError(error.ErrorMessage);
            return CustomResponse();
        }

        protected ActionResult CustomResponse(ResponseResult response)
        {
            ResponseHasErrors(response);
            return CustomResponse();
        }

        protected bool ResponseHasErrors(ResponseResult response)
        {
            if (response is null || !response.Errors.Messages.Any()) return false;

            foreach (var error in response.Errors.Messages)
                AddProcessError(error);
            
            return true;
        }

        protected bool IsValidOperation() => !Errors.Any();
        protected void AddProcessError(string error) => Errors.Add(error);
        protected void AddProcessErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                Errors.Add(error);
            }
        }
        protected void ClearProcessErrors() => Errors.Clear();
    }
}
