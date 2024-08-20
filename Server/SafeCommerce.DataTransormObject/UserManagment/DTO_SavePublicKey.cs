using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.UserManagment;

public class DTO_SavePublicKey
{
    [Required]
    public string PublicKey { get; set; }  = string.Empty;
    [Required]
    public string Signature { get; set; }  = string.Empty;
    public string HintPassPhrase { get; set; }  = string.Empty;
    [Required]
    public string SigningPublicKey { get; set; } = string.Empty;
}