using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.AccountManagment;

public class ClientDto_UserChangePassword
{
    [Required(ErrorMessage = "Old password is required"), DataType(DataType.Password)]
    public string OldPassword { get; set; } = string.Empty;
    [Required(ErrorMessage = "New password is required"), DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;
    [Required(ErrorMessage = "Confirm new password is required"), DataType(DataType.Password), Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}