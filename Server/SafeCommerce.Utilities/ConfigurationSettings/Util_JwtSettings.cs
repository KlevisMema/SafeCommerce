/*
 * Represents the configuration settings required for JWT token generation.
 * It includes settings such as audience, secret key, issuer, and token lifetime.
*/

namespace SafeCommerce.Utilities.ConfigurationSettings;

/// <summary>
/// Represents the authentication settings for JWT tokens.
/// </summary>
public class Util_JwtSettings
{
    /// <summary>
    /// The name of the section in the configuration file.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// The audience of the JWT token.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// The secret key used for signing the JWT token.
    /// </summary>
    public string Key { get; set; } = string.Empty;
    /// <summary>
    /// The secret key used for signing the JWT confrim login token.
    /// </summary>
    public string KeyConfrimLogin { get; set; } = string.Empty;

    /// <summary>
    /// The issuer of the JWT token.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// The lifetime of the JWT token in hours.
    /// </summary>
    public int LifeTime { get; set; } = 0;
}