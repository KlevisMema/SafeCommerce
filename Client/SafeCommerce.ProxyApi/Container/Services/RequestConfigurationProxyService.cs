using SafeCommerce.ProxyApi.Helpers.Constants;
using SafeCommerce.ProxyApi.Container.Interfaces;

namespace SafeCommerce.ProxyApi.Container.Services;

internal class RequestConfigurationProxyService
(
    IConfiguration configurations,
    ILogger<RequestConfigurationProxyService> logger
) : IRequestConfigurationProxyService
{
    private string? Client;
    private string? BaseAddrOfMainApi;

    public string
    GetBaseAddrOfMainApi()
    {
        try
        {
            SetBaseAddrOfMainApi();

            return BaseAddrOfMainApi!;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Something went wrong in 'RequestConfigurationProxyService'");
            throw;
        }
    }

    public string
    GetClient()
    {
        try
        {
            SetClient();
            return Client!;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Something went wrong in 'RequestConfigurationProxyService'");

            throw;
        }
    }

    private void
    SetClient()
    {
        Client = configurations.GetSection(API_Helper_Const_Request.ProxyClientName).Value
            ?? throw new ArgumentNullException("Client name is missing, check configuration settings");
    }

    private void
    SetBaseAddrOfMainApi()
    {
        BaseAddrOfMainApi = configurations.GetSection(API_Helper_Const_Request.MainApiBaseAddr).Value
            ?? throw new ArgumentNullException("Base addr of main api is missing, check configuration settings");
    }
}