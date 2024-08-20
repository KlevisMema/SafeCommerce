using System.Text.Json.Serialization;

namespace SafeCommerce.Utilities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    User = 0,
    Moderator = 1,
}