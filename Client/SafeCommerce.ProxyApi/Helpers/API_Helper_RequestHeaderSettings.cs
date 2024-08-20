namespace SafeCommerce.ProxyApi.Helpers;

public class API_Helper_RequestHeaderSettings
{
    public const string SectionName = "RequestHeaderSettings";

    public string Bearer { get; set; } = string.Empty;
    public string ClientIP { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string XSRF_TOKEN { get; set; } = string.Empty;
    public string RefreshAuthToken { get; set; } = string.Empty;
    public string Client_XSRF_TOKEN { get; set; } = string.Empty;
    public string RefreshAuthTokenId { get; set; } = string.Empty;
    public string AspNetCoreAntiforgery { get; set; } = string.Empty;
}