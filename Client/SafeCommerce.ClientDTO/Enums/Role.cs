using System.Text.Json.Serialization;

namespace SafeCommerce.ClientDTO.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    User = 0,
    Moderator = 1,
    NoRole = 2,
}