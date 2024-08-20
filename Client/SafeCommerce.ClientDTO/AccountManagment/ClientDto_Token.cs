namespace SafeCommerce.ClientDTO.AccountManagment;

public class ClientDto_Token
{
    public string? Token { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public Guid RefreshTokenId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime ValididtyTime { get; set; }
}