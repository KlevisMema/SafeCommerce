/* 
 * Contains configuration settings related to confirming a user's registration.
 * This class maps to a specific section in the configuration file, enabling streamlined access 
 * to settings pertinent to registration confirmation processes.
 */

namespace SafeCommerce.Utilities.ConfigurationSettings;

/// <summary>
/// Represents configuration settings specific to confirming a user's registration.
/// </summary>
public class Util_ConfirmRegistrationSettings
{
    /// <summary>
    /// The section name in the configuration file for registration confirmation settings.
    /// </summary>
    public const string SectionName = "ConfirmRegistration";
    /// <summary>
    /// The route or URL to be used in the communication for registration confirmation.
    /// </summary>
    public string Route { get; set; } = string.Empty;
}