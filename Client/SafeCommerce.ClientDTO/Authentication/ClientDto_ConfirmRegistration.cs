using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Authentication;
public class ClientDto_ConfirmRegistration
{
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}