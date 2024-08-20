/*
    * This interface provides the signature for the registration 
    * functionality within the Authentication module. It ensures 
    * that any implementing class provides a method for registering 
    * users based on the provided DTO.
*/

using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Authentication;

namespace SafeCommerce.Authentication.Interfaces;

/// <summary>
/// Defines the contract for the registration operations within the Authentication module.
/// </summary>
public interface IAUTH_Register
{
    /// <summary>
    /// Registers a new user based on the provided registration data transfer object.
    /// </summary>
    /// <param name="registerDto">The data transfer object containing user registration details.</param>
    /// <returns>A generic response indicating the success or failure of the registration operation.</returns>
    Task<Util_GenericResponse<bool>>
    RegisterUser
    (
        DTO_Register registerDto
    );
    /// <summary>
    /// Confirms user registration
    /// </summary>
    /// <param name="confirmRegistrationDto"> The <see cref="DTO_ConfirmRegistration"/> object dto </param>
    /// <returns>A generic response indicating the success or failure of the registration confirmation.</returns>
    Task<Util_GenericResponse<bool>>
    ConfirmRegistration
    (
        DTO_ConfirmRegistration confirmRegistrationDto
    );
    /// <summary>
    /// Re confirms a user if the user forgot the check his email 
    /// to confirm his registration.
    /// </summary>
    /// <param name="email">The email of the user</param>
    /// <returns>A generic response indicating the success or failure of the re registration confirmation.</returns>
    Task<Util_GenericResponse<bool>>
    ReConfirmRegistrationRequest
    (
        string email
    );
}