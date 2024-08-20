using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Security;

namespace SafeCommerce.Authentication.Interfaces;

public interface IAUTH_RefreshToken
{
    /// <summary>
    /// Refreshes a token of a expired jwt token
    /// </summary>
    /// <param name="validateTokenDto">The <see cref="DTO_ValidateToken"/> object</param>
    /// <returns> A generic response indicating if the operation ended successfully with the new token or not </returns>
    Task<Util_GenericResponse<DTO_Token>> 
    RefreshToken
    (
        DTO_ValidateToken validateTokenDto
    );
}