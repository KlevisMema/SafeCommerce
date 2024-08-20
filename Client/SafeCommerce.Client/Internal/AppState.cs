using SafeCommerce.ClientDTO.Authentication;

namespace SafeCommerce.Client.Internal;

public class AppState
{
    private ClientDto_LoginResult? ClientSecrests { get; set; }


    /// <summary>
    /// A property indicating that user have public key in the database but no keys in his 
    /// browser this means that user is logging form a new device or has cleared his browser so 
    /// his keys needs to be generated again and saved in the browser.
    /// </summary>
    public bool GenerateSameKeys { get; set; } = false;

    public void
    SetClientSecrets
    (
        ClientDto_LoginResult? clientSecrest
    )
    {
        ClientSecrests = clientSecrest;
    }

    public ClientDto_LoginResult?
    GetClientSecrets()
    {
        return ClientSecrests;
    }

    public event Action? OnLogOut;

    public void LogOut() => OnLogOut?.Invoke();
}