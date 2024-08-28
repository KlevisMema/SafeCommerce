/* 
 * Contains route definitions for various aspects of the SafeCommerce client-server communication.
 */

namespace SafeCommerce.ClientServerShared.Routes;

/// <summary>
/// Base routes for the SafeCommerce application.
/// </summary>
public static class BaseRoute
{
    public const string Route = "api/[controller]";
    public const string RouteShopForClient = "api/Shop/";
    public const string RouteItemForClient = "api/Item/";
    public const string RouteModeration = "api/Moderation/";
    public const string RouteInvitationForClient = "api/Invitation/";
    public const string RouteAuthenticationForClient = "api/Authentication/";
    public const string RouteAccountManagmentForClient = "api/AccountManagment/";

    public const string RouteItemProxy = "api/ItemProxy/";
    public const string RouteShopProxy = "api/ShopProxy/";
    public const string RouteAuthenticationProxy = "api/AuthProxy/";
    public const string ProxyRouteModeration = "api/ModerationProxy/";
    public const string RouteAccountManagmentProxy = "api/AccountManagmentProxy/";
    public const string RouteProxyShopInvitationForClient = "api/ShopInvitatonProxy/";
    public const string RouteProxyItemInvitationForClient = "api/ItemInvitatonProxy/";
    public const string ProxyRouteAccountManagmentForClient = "api/AccountManagmentProxy/";
    public const string ProxyRouteCheckOut = "api/CheckOutProxy/";
}