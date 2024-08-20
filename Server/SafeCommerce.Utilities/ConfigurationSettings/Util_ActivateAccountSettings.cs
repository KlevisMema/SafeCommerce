/* 
 * Contains configuration settings related to activating a user's account.
 * This class maps to a specific section in the configuration file, allowing for easy retrieval and management of settings related to account activation.
 */

namespace SafeCommerce.Utilities.ConfigurationSettings;

/// <summary>
/// Represents configuration settings specific to activating a user's account.
/// </summary>
public class Util_ActivateAccountSettings
{
    /// <summary>
    /// The section name in the configuration file for account activation settings.
    /// </summary>
    public const string SectionName = "ActivateAccount";
    /// <summary>
    /// The route or URL to be used in the communication for account activation.
    /// </summary>
    public string Route { get; set; } = string.Empty;
    /// <summary>
    /// A reason or context provided for the account activation process.
    /// </summary>
    public string Reason { get; set; } = string.Empty;
}