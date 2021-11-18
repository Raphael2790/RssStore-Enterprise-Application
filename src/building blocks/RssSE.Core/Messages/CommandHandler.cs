using FluentValidation.Results;
using RssSE.Core.Data;
using System.Threading.Tasks;

namespace RssSE.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        public CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string errorMessage) => ValidationResult.Errors.Add(new ValidationFailure(string.Empty, errorMessage));

        protected async Task<ValidationResult> PersistData(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AddError("Houve um erro ao persistir os dados");
            return ValidationResult;
        }
    }
}
