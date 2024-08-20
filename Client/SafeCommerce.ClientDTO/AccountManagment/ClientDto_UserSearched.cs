namespace SafeCommerce.ClientDTO.AccountManagment;

public class ClientDto_UserSearched
{
    public string UserName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public string SigningPublicKey { get; set; } = string.Empty;
}