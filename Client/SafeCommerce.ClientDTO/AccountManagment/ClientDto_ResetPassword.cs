using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.AccountManagment;

public class ClientDto_ResetPassword
{
    [NoXss]
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}