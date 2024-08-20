using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.AccountManagment;

public class ClientDto_ForgotPassword
{
    [NoXss]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}