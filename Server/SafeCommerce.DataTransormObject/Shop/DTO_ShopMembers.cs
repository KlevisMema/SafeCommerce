namespace SafeCommerce.DataTransormObject.Shop;

public class DTO_ShopMembers
{
    public string UserId { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public string SigningPublicKey { get; set; } = string.Empty;
}