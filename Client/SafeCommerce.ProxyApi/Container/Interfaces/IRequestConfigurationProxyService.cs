namespace SafeCommerce.ProxyApi.Container.Interfaces;

public interface IRequestConfigurationProxyService
{
    string
    GetBaseAddrOfMainApi();

    string
    GetClient();
}