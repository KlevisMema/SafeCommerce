namespace SafeCommerce.ProxyApi.Helpers;

internal static class API_Helper_ParamsStringChecking
{
    public static void
    CheckNullOrEmpty
    (
        params (string ParamName, string? ParamValue)[] parameters
    )
    {
        foreach (var (ParamName, ParamValue) in parameters)
        {
            if (string.IsNullOrWhiteSpace(ParamValue))
                throw new ArgumentNullException(ParamName, $"The parameter '{ParamName}' cannot be null or empty.");
        }
    }
}