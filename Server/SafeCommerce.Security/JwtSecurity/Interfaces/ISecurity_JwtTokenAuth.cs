/* 
 * Defines the interface IOAuthJwtTokenService for creating JWT (JSON Web Tokens) tokens. 
 * This interface outlines the contract for JWT token generation services and is designed to be generic for flexibility in different authentication contexts.
 */


namespace SafeCommerce.Security.JwtSecurity.Interfaces;

/// <summary>
/// Defines a contract for services that generate JWT (JSON Web Tokens) for authentication purposes.
/// This interface is generic, allowing various implementations to specify their types for input parameters and return types.
/// </summary>
/// <typeparam name="TService">The type of the service implementing this interface.</typeparam>
/// <typeparam name="TFuncInputParamType">The type of the input parameter for the token creation function.</typeparam>
/// <typeparam name="TReturnType">The type of the return value from the token creation function.</typeparam>
public interface ISecurity_JwtTokenAuth<TService, TFuncInputParamType, TReturnType>
{
    /// <summary>
    /// Creates a JWT token based on the specified input parameter.
    /// </summary>
    /// <param name="user">The input parameter used for token generation. This could be user information or any other required data.</param>
    /// <returns>A task representing the asynchronous operation. The task result is of type <see cref="TReturnType"/>, representing the generated JWT token.</returns>
    Task<TReturnType>
    CreateToken
    (
        TFuncInputParamType user
    );
}