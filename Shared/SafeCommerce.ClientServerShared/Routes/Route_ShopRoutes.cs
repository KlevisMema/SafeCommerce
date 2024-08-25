/*
 * Contains route definitions for various aspects of the SafeCommerce client-server communication.
 */

namespace SafeCommerce.ClientServerShared.Routes;

/// <summary>
/// Routes for shop management in the SafeCommerce application.
/// </summary>
public static class Route_ShopRoutes
{
    public const string GetPublicShops = "PublicShops/{userId}"; 
    public const string GetShop = "PublicShops/{shopId}/{userId}"; 
    public const string CreateShop = "CreateShop/{userId}";
    public const string EditShop = "EditShop/{shopId}/user/{userId}";
    public const string DeleteShop = "DeleteShop/{shopId}/user/{userId}"; 
    public const string GetShops = "UserShops/{userId}";
    public const string InviteUserToShop = "InviteUser/{userId}/ToShop/{shopId}";
    public const string GetShopsForModeration = "ShopsForModeration/{userId}";
    public const string ModerateShop = "ModerateShop/{userId}";
    public const string RemoveUserFromShop = "RemoveUserFromShop/{userId}";
    public const string RemoveUserFromItem = "RemoveUserFromItem/{userId}";
    public const string GetMembersOfTheShop = "GetMembersOfTheShop/{shopId}/{userId}";
    public const string GetMembersOfTheItem = "GetMembersOfTheItem/{itemId}/{userId}";

    public const string ProxyGetPublicShops = "PublicShops";
    public const string ProxyGetShop = "ShopDetails/{shopId}";
    public const string ProxyCreateShop = "CreateShop";
    public const string ProxyEditShop = "EditShop/{shopId}";
    public const string ProxyDeleteShop = "DeleteShop/{shopId}";
    public const string ProxyGetShops = "UserShops";
    public const string ProxyInviteUserToShop = "InviteUserToShop/{shopId}";
    public const string ProxyGetShopsForModeration = "ShopsForModeration";
    public const string ProxyModerateShop = "ModerateShop";
    public const string ProxyRemoveUserFromShop = "RemoveUserFromShop";
    public const string ProxyRemoveUserFromItem = "RemoveUserFromItem";
    public const string ProxyGetMembersOfTheShop = "GetMembersOfTheShop/{shopId}";
    public const string ProxyGetMembersOfTheItem = "GetMembersOfTheItem/{itemId}";
}