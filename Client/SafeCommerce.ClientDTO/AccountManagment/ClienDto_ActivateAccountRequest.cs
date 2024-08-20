using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.AccountManagment;

public class ClienDto_ActivateAccountRequest
{
    [NoXss]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}