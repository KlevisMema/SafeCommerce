/*
 * Defines the data transfer object for authenticated users.
 * This DTO provides details of an authenticated user in the system.
*/

using SafeCommerce.Utilities.Enums;

namespace SafeCommerce.DataTransormObject.Authentication;

/// <summary>
/// Represents an authenticated user with basic details and roles.
/// </summary>
public class DTO_AuthUser
{
    /// <summary>
    /// Gets or sets the gender of the authenticated user.
    /// </summary>
    public Gender Gender { get; set; }
    /// <summary>
    /// Gets or sets the birthday of the authenticated user.
    /// </summary>
    public DateTime BirthDay { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier for the authenticated user.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the email address of the authenticated user.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the full name of the authenticated user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the last name of the authenticated user.
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the list of roles assigned to the authenticated user.
    /// </summary>
    public List<string> Roles { get; set; } = Enumerable.Empty<string>().ToList();
}