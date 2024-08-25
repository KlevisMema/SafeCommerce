namespace SafeCommerce.ClientServerShared.Routes;

public static class Route_InvitationRoute
{
    public const string SendInvitation = "SendInvitation/{userId}";
    public const string AcceptInvitation = "AcceptInvitation/{userId}";
    public const string RejectInvitation = "RejectInvitation/{userId}";
    public const string DeleteInvitation = "DeleteInvitation/{userId}";
    public const string GetShopsInvitations = "GetShopsInvitations/{userId}";
    public const string GetSentShopInvitations = "GetSentShopInvitations/{userId}";

    public const string ProxySendInvitation = "SendInvitation";
    public const string ProxyAcceptInvitation = "AcceptInvitation";
    public const string ProxyRejectInvitation = "RejectInvitation";
    public const string ProxyDeleteInvitation = "DeleteInvitation";
    public const string ProxyGetShopsInvitations = "GetShopsInvitations";
    public const string ProxyGetSentShopInvitations = "GetSentShopInvitations";


    public const string SendItemInvitation = "SendItemInvitation/{userId}";
    public const string AcceptItemInvitation = "AcceptItemInvitation/{userId}";
    public const string RejectItemInvitation = "RejectItemInvitation/{userId}";
    public const string DeleteItemInvitation = "DeleteItemInvitation/{userId}";
    public const string GetItemsInvitations = "GetItemsInvitations/{userId}";
    public const string GetSentItemsInvitations = "GetSentItemsInvitations/{userId}";

    public const string ProxySendItemInvitation = "SendItemInvitation";
    public const string ProxyAcceptItemInvitation = "AcceptItemInvitation";
    public const string ProxyRejectItemInvitation = "RejectItemInvitation";
    public const string ProxyDeleteItemInvitation = "DeleteItemInvitation";
    public const string ProxyGetItemsInvitations = "GetItemsInvitations";
    public const string ProxyGetSentItemsInvitations = "GetSentItemsInvitations";
}