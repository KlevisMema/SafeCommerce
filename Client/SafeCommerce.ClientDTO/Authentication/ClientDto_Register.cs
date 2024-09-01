using SafeCommerce.ClientDTO.Enums;
using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Authentication;

public class ClientDto_Register
{
    [NoXss]
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Invalid username length")]
    public string Username { get; set; } = string.Empty;

    [NoXss]
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Invalid fullname length")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateTime? Birthday { get; set; }

    [Required]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }

    [NoXss]
    [Required, EmailAddress(ErrorMessage = "Invalid E-mail address")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Invalid password length")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Invalid password length")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public bool Enable2FA { get; set; } = false;
}