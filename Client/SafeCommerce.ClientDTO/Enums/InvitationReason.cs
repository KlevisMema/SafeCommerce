using System.Text.Json.Serialization;

namespace SafeCommerce.ClientDTO.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InvitationReason
{
    ShopInvitation = 0,
    UserInvitation = 1,
}