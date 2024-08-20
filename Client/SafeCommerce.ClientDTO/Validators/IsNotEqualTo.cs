using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Validators;

internal class IsNotEqualTo : ValidationAttribute
{
    private readonly string _comparisonProperty;

    internal IsNotEqualTo
    (
        string comparisonProperty
    )
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult
    IsValid
    (
        object? value,
        ValidationContext validationContext
    )
    {
        string? currentValue = value as string;

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            throw new ArgumentException("Property with this name not found");

        string? comparisonValue = property.GetValue(validationContext.ObjectInstance) as string;

        if (currentValue != null && comparisonValue != null && currentValue.Equals(comparisonValue))
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success!;
    }
}