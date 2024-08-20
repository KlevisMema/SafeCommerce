/*
 * The AUTH_Register class is part of the Authentication module and provides functionalities
 * for user registration. This includes the main process of registering a user and
 * assigning roles to the newly registered user. Error handling and logging are integral
 * to these operations to ensure smooth user registration flow.
 */

using AutoMapper;
using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.IP;
using SafeCommerce.Utilities.Log;
using SafeCommerce.Utilities.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SafeCommerce.Utilities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.Utilities.Responses;
using Microsoft.Extensions.Configuration;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.Authentication.Interfaces;
using SafeCommerce.Utilities.ConfigurationSettings;
using SafeCommerce.DataTransormObject.Authentication;

namespace SafeCommerce.Authentication.Auth;

/// <summary>
/// Handles user registration within the authentication module.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AUTH_Register"/> class.
/// </remarks>
/// <param name="logger">The logger instance.</param>
/// <param name="mapper">The AutoMapper instance.</param>
/// <param name="configuration"> The configurations </param>
/// <param name="db">The application's database context.</param>
/// <param name="userManager">The user manager instance.</param>
/// <param name="httpContextAccessor">The HttpContext accessor instance.</param>
/// <param name="confirmRegistrationSettings">The confirm registration settings</param>
public class AUTH_Register
(
    IMapper mapper,
    ApplicationDbContext db,
    ILogger<AUTH_Register> logger,
    IConfiguration configuration,
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<Util_ConfirmRegistrationSettings> confirmRegistrationSettings
) : Util_BaseAuthDependencies<AUTH_Register, ApplicationUser, ApplicationDbContext>(
    mapper,
    logger,
    httpContextAccessor,
    userManager,
    configuration,
    db
), IAUTH_Register
{
    /// <summary>
    /// Registers a new user to the application.
    /// </summary>
    /// <param name="registerDto">The data transfer object containing user registration details.</param>
    /// <returns>A generic response indicating the success or failure of the registration.</returns>
    public async Task<Util_GenericResponse<bool>>
    RegisterUser
    (
        DTO_Register registerDto
    )
    {
        try
        {
            var mappedUser = _mapper.Map<ApplicationUser>(registerDto);

            var createUserResult = await _userManager.CreateAsync(mappedUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [RegisterUser Method] => [IP] {IP} 
                        user was not created Created =>  [RESULT] : {@createUserResult} 
                        and {@createUserResultErrors}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    createUserResult,
                    createUserResult.Errors.Select(x => x.Description)
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong when creating the account!",
                    createUserResult.Errors.Select(x => x.Description).ToList(),
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            var assignRole = await AssignUserToUserRole(registerDto.UserName);

            if (!assignRole.Succsess)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [RegisterUser Method] => [IP] {IP}    
                        user was not assigned to a role =>  [RESULT] : {@assignRole}. 
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    assignRole
                );

                return assignRole;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(mappedUser);

            var encodedToken = System.Net.WebUtility.UrlEncode(token);

            var route = confirmRegistrationSettings.Value.Route.Replace("{token}", encodedToken).Replace("{email}", mappedUser.Email);

            var emailResult = await Util_Email.SendEmailForRegistrationConfirmation(mappedUser.Email!, route, mappedUser.FullName);

            if (!emailResult.IsSuccessStatusCode)
            {
                _logger.Log
                (
                     LogLevel.Error,
                     """
                            [Authentication Module]-[AUTH_Register Class]-[RegisterUser Method] => 
                            [IP] {IP} a token for user with email {registerDto.Email} 
                            was successfully issued but the email send failed. {@emailResult}
                      """,
                     await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                     registerDto.Email,
                     emailResult
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong, please try again",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            _logger.LogInformation
            (
                """
                    [Authentication Module]-[AUTH_Register Class]-[RegisterUser Method] =>, 
                    [IP] {IP} | user with email {Email} was successfully created. 
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                registerDto.Email
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                """
                    Your account was successfully created,
                    we have send you an email to confirm your account.
                 """,
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AUTH_Register>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [Authentication Module]-[AUTH_Register Class]-[RegisterUser Method],   
                    user with [EMAIL] {registerDto.Email}.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Confirms user registration
    /// </summary>
    /// <param name="confirmRegistrationDto"> The <see cref="DTO_ConfirmRegistration"/> object dto </param>
    /// <returns>A generic response indicating the success or failure of the registration confirmation.</returns>
    public async Task<Util_GenericResponse<bool>>
    ConfirmRegistration
    (
        DTO_ConfirmRegistration confirmRegistrationDto
    )
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(confirmRegistrationDto.Email);

            if (user is null)
            {
                _logger.Log
                (
                   LogLevel.Error,
                   """
                        [Authentication Module]-[AUTH_Register Class]-[ConfirmRegistration Method] => 
                        [IP] {IP}, user with email {Email} doesn't exists.
                    """,
                   await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                   confirmRegistrationDto.Email
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "User doesn't exists",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            if (user.EmailConfirmed)
            {
                _logger.Log
                (
                  LogLevel.Error,
                  """
                        [Authentication Module]-[AUTH_Register Class]-[ConfirmRegistration Method] => 
                        [IP] {IP}, user with email {Email} is already confirmed.
                   """,
                  await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                  confirmRegistrationDto.Email
                );

                return Util_GenericResponse<bool>.Response
                (
                    true,
                    true,
                    "Account is already confirmed",
                    null,
                    System.Net.HttpStatusCode.OK
                );
            }

            user.ModifiedAt = DateTime.UtcNow;

            var verifyToken = await _userManager.ConfirmEmailAsync(user, confirmRegistrationDto.Token);

            var userRole = await roleManager.FindByNameAsync(Role.User.ToString());

            if (userRole is not null && userRole.Name is not null)
                await _userManager.AddToRoleAsync(user, userRole.Name);

            if (!verifyToken.Succeeded)
            {
                _logger.Log
                (
                   LogLevel.Error,
                   """
                        [Authentication Module]-[AUTH_Register Class]-[ConfirmRegistration Method] => 
                        [IP] {IP} user with email {Email}
                        tried to confirm the registration but the failed on the token validation. {@verifyTokenErrors}. 
                    """,
                   await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                   confirmRegistrationDto.Email,
                   verifyToken.Errors.Select(x => x.Description)
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong, token not valid.",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "Email successfully validated",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AUTH_Register>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [Authentication Module]-[AUTH_Register Class]-[ConfirmRegistration Method], 
                    user with  [Email] {confirmRegistrationDto.Email} tried to verify his registration.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Re confirms a user if the user forgot the check his email 
    /// to confirm his registration.
    /// </summary>
    /// <param name="email">The email of the user</param>
    /// <returns>A generic response indicating the success or failure of the re registration confirmation.</returns>
    public async Task<Util_GenericResponse<bool>>
    ReConfirmRegistrationRequest
    (
        string email
    )
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.Log
                (
                  LogLevel.Error,
                  """
                        [Authentication Module]-[AUTH_Register Class]-[ReConfirmRegistrationRequest Method] => 
                        [IP] {IP} user with email {email} doesn't exists.
                   """,
                  await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                  email
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "User doesn't exists",
                    null, System.Net.HttpStatusCode.NotFound
                );
            }

            if (user.IsDeleted)
            {
                _logger.Log
                (
                  LogLevel.Error,
                  """
                        [Authentication Module]-[AUTH_Register Class]-[ReConfirmRegistrationRequest Method] => 
                        [IP] {IP} user with email {email} account is disabled.
                   """,
                  await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                  email
                );

                return Util_GenericResponse<bool>.Response(false, false, "User doesn't exists", null, System.Net.HttpStatusCode.NotFound);
            }

            if (user.EmailConfirmed)
            {
                _logger.Log
                (
                  LogLevel.Error,
                  """
                        [Authentication Module]-[AUTH_Register Class]-[ReConfirmRegistrationRequest Method] => 
                        [IP] {IP} user with email {email} account already confirmed.
                   """,
                  await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                  email
                 );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Account is already confirmed.",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = System.Net.WebUtility.UrlEncode(token);

            var route = confirmRegistrationSettings.Value.Route.Replace("{token}", encodedToken).Replace("{email}", user.Email);

            var emailResult = await Util_Email.SendEmailForRegistrationConfirmation(user.Email!, route, user.FullName);

            if (!emailResult.IsSuccessStatusCode)
            {
                _logger.Log
                (
                  LogLevel.Error,
                  """
                        [Authentication Module]-[AUTH_Register Class]-[ReConfirmRegistrationRequest Method] => 
                        [IP] {IP} a token for user with email {email} 
                        was successfully issued but the email send failed. {@emailResult}
                   """,
                  await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                  email,
                  emailResult
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong, please try again",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            _logger.Log
            (
                LogLevel.Error,
                """
                    [Authentication Module]-[AUTH_Register Class]-[ReConfirmRegistrationRequest Method] => 
                    [IP] {IP} a token for user with email {email} 
                    was successfully and the email was successfully sent.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                email
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "An email has been sent to confirm your account",
                null,
                System.Net.HttpStatusCode.OK
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AUTH_Register>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [Authentication Module]-[AUTH_Register Class]-[ReConfirmRegistrationRequest Method], 
                    user with [Email] {email} tried to verify his registration.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Assigns a registered user to a predefined 'User' role.
    /// </summary>
    /// <param name="userName">The username of the registered user.</param>
    /// <returns>A generic response indicating the success or failure of the role assignment.</returns>
    private async Task<Util_GenericResponse<bool>>
    AssignUserToUserRole
    (
        string userName
    )
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);

            var roles = await _db.Roles.ToListAsync();

            if (roles.Count > 0 && user is not null)
            {
                string? userRole = roles?.FirstOrDefault(role => role.Name == "User")?.Name;

                if (String.IsNullOrEmpty(userRole))
                {
                    await _userManager.DeleteAsync(user);

                    return Util_GenericResponse<bool>.Response
                    (
                        false,
                        false,
                        """
                            Something went wrong in assigning the role to the user,
                            user was not created!"
                         """,
                        null,
                        System.Net.HttpStatusCode.BadRequest
                    );
                }

                var addToRole = await _userManager.AddToRoleAsync(user!, userRole);

                if (addToRole is null)
                {
                    await _userManager.DeleteAsync(user);

                    return Util_GenericResponse<bool>.Response
                    (
                        false,
                        false,
                        """
                            Something went wrong in assigning the role to the user,
                            user was not created!
                         """,
                        null,
                        System.Net.HttpStatusCode.BadRequest
                    );
                }
            }

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "User successfully assigned to user role",
                null,
                System.Net.HttpStatusCode.BadRequest
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AUTH_Register>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [Authentication Module] - [AssignUserToUserRole Method], "
                    user with [UserName] {userName}",
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
}