namespace SafeCommerce.DataTransormObject.Authentication;

public class DTO_ConfirmLogin
{
    public Guid UserId { get; set; }
    public string OTP { get; set; } = string.Empty;
}