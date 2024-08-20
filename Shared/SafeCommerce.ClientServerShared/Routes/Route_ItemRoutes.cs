/*
 * Contains route definitions for various aspects of the SafeCommerce client-server communication.
 */

namespace SafeCommerce.ClientServerShared.Routes;

/// <summary>
/// Routes for item management in the SafeCommerce application.
/// </summary>
public static class Route_ItemRoutes
{
    public const string GetItemDetails = "ItemDetails/{userId}/{itemId}";
    public const string CreateItem = "CreateItem/{userId}";
    public const string EditItem = "EditItem/{userId}/{itemId}";
    public const string DeleteItem = "DeleteItem/{userId}/{itemId}";
    public const string GetItemsByShopId = "ItemsByShop/{userId}/{shopId}";
    public const string ShareItem = "ShareItem/{userId}";
    public const string ModerateItem = "ModerateItem/{userId}";
    public const string GetItemsForModeration = "ItemsForModeration/{userId}";
    public const string GetUserItems = "UserItems/{userId}";

    public const string ProxyGetItemDetails = "ItemDetails/{itemId}";
    public const string ProxyCreateItem = "CreateItem";
    public const string ProxyEditItem = "EditItem/{itemId}";
    public const string ProxyDeleteItem = "DeleteItem/{itemId}";
    public const string ProxyGetItemsByShopId = "ItemsByShop/{shopId}";
    public const string ProxyShareItem = "ShareItem";
    public const string ProxyModerateItem = "ModerateItem";
    public const string ProxyGetUserItems = "UserItems";
    public const string PeoxyGetItemsForModeration = "ItemsForModeration";
}
