/* 
 * Contains configuration settings related to resetting a user's password.
 * This class maps to a specific section in the configuration file, facilitating easy access 
 * to settings essential for the password reset process.
 */

namespace SafeCommerce.Utilities.ConfigurationSettings;

/// <summary>
/// Represents configuration settings specific to resetting a user's password.
/// </summary>
public class Util_ResetPasswordSettings
{
    /// <summary>
    /// The section name in the configuration file for password reset settings.
    /// </summary>
    public const string SectionName = "ResetPassword";
    /// <summary>
    /// The route or URL to be used in the communication for password reset.
    /// </summary>
    public string Route { get; set; } = string.Empty;
}