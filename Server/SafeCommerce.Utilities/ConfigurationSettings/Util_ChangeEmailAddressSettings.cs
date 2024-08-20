/* 
 * Contains configuration settings related to changing a user's email address.
 * This class maps to a specific section in the configuration file, allowing easy access to related settings.
 */

namespace SafeCommerce.Utilities.ConfigurationSettings;

/// <summary>
/// Represents configuration settings specific to changing a user's email address.
/// </summary>
public class Util_ChangeEmailAddressSettings
{
    /// <summary>
    /// The section name in the configuration file for change email address settings.
    /// </summary>
    public const string SectionName = "ChangeEmailAddress";
    /// <summary>
    /// The route or URL to be used in the email for changing the email address.
    /// </summary>
    public string Route { get; set; } = string.Empty;
}