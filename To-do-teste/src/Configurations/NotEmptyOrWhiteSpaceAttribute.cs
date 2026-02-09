using System.ComponentModel.DataAnnotations;

namespace To_do_teste.src.Configurations
{
    public class NotEmptyOrWhiteSpaceAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str && string.IsNullOrWhiteSpace(str))
                return new ValidationResult(ErrorMessage ?? "Campo inválido");

            return ValidationResult.Success;
        }
    }
}
