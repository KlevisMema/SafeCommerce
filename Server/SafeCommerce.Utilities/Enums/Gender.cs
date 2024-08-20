/*
 * Enumerates the different genders for a user.
 * Supports JSON serialization using a string representation of the enum values.
*/

using System.Text.Json.Serialization;

namespace SafeCommerce.Utilities.Enums;

/// <summary>
/// Represents the gender of a user.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    /// <summary>
    /// Represents a male gender. Value: 1.
    /// </summary>
    Male = 1,
    /// <summary>
    /// Represents a female gender. Value: 2.
    /// </summary>
    Female = 2
}