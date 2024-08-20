using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Authentication;

public class ClientDto_PassPhrase
{
    [Required(ErrorMessage = "Passphrase is required")]
    [Length(6, 100, ErrorMessage = "Passphrase length invalid")]
    public string SecretPassPhrase { get; set; } = string.Empty;

    public string Hint { get; set; } = string.Empty;

    public bool ClearKeys { get; set; } = false;

    public bool MyDevice { get; set; } = false;
}