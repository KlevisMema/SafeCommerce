namespace SafeCommerce.DataTransormObject.UserManagment;

public class DTO_UserSearched
{
    public byte[]? UserImage { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public string SigningPublicKey { get; set; } = string.Empty;
}