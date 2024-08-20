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
    public const string RouteAuthenticationForClient = "api/Authentication/";
    public const string RouteAccountManagmentForClient = "api/AccountManagment/";
    public const string RouteItemForClient = "api/Item/";
    public const string RouteShopForClient = "api/Shop/";
    public const string RouteShopInvitationForClient = "api/Invitation/";

    public const string RouteAuthenticationProxy = "api/AuthProxy/";
    public const string RouteProxyShopInvitationForClient = "api/ShopInvitatonProxy/";
    public const string RouteAccountManagmentProxy = "api/AccountManagmentProxy/";
    public const string RouteItemProxy = "api/ItemProxy/";
    public const string RouteShopProxy = "api/ShopProxy/";
}