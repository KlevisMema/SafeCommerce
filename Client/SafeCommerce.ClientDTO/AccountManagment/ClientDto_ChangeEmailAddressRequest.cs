using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.AccountManagment;

public class ClientDto_ChangeEmailAddressRequest
{
    [NoXss]
    [Required, EmailAddress]
    public string CurrentEmailAddress { get; set; } = string.Empty;

    [NoXss]
    [Required, EmailAddress, IsNotEqualTo("CurrentEmailAddress")]
    public string NewEmailAddress { get; set; } = string.Empty;

    [NoXss]
    [Required, EmailAddress, IsNotEqualTo("CurrentEmailAddress")]
    public string ConfirmNewEmailAddress { get; set; } = string.Empty;
}