using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.AccountManagment;

public class ClientDto_ActivateAccountConfirmation
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Token { get; set; } = string.Empty;
}