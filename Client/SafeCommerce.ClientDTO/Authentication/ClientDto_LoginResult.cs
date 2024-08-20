namespace SafeCommerce.ClientDTO.Authentication;

public class ClientDto_LoginResult
{
    public ClientDto_Token Token { get; set; } = new();
    public bool RequireOtpDuringLogin { get; set; } = false;
    public string UserId { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public bool GenerateKeys { get; set; } = false;
}