using FluentValidation.Results;

namespace Inz.OneOfHelper
{
    public class NotValidateResponse
    {
        public ValidationResult ValidationResult;
        public NotValidateResponse(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }
    }
}
