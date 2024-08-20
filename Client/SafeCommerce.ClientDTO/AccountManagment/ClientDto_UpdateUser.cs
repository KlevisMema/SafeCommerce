using SafeCommerce.ClientDTO.Enums;
using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.AccountManagment;

public class ClientDto_UpdateUser
{
    [NoXss]
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [NoXss]
    [Required]
    public DateTime Birthday { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [NoXss]
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool Enable2FA { get; set; } = false;
}