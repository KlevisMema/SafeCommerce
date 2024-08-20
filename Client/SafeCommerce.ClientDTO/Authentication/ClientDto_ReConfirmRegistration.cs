using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Authentication;

public class ClientDto_ReConfirmRegistration
{
    [NoXss]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}