/*
 * This file defines the IsNotEqualTo validation attribute class in the SafeCommerce.DataTransormObject.Validators namespace.
 * The IsNotEqualTo attribute is used to validate that the target property's value is not equal to the specified comparison property's value.
 */

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Validators;

/// <summary>
/// Validation attribute to ensure that the target property's value is not equal to the specified comparison property's value.
/// </summary>
internal class IsNotEqualTo : ValidationAttribute
{
    /// <summary>
    /// The name of the property to compare against.
    /// </summary>
    private readonly string _comparisonProperty;
    /// <summary>
    /// Initializes a new instance of the <see cref="IsNotEqualTo"/> class.
    /// </summary>
    /// <param name="comparisonProperty">The name of the property to compare against.</param>
    internal IsNotEqualTo
    (
        string comparisonProperty
    )
    {
        _comparisonProperty = comparisonProperty;
    }
    /// <summary>
    /// Validates that the target property's value is not equal to the specified comparison property's value.
    /// </summary>
    /// <param name="value">The value of the target property being validated.</param>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>
    /// <see cref="ValidationResult"/> if the validation fails and the target property's value is equal to the comparison property's value; otherwise, <see cref="ValidationResult.Success"/>.
    /// </returns>
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