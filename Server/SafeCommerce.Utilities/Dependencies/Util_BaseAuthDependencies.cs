/*
    * This utility class extends the base utility dependencies with additional 
    * authentication-specific dependencies. Specifically, it incorporates the UserManager
    * for handling user-related operations in conjunction with identity management. 
    * By providing this extension, services that deal with user authentication and management
    * can utilize the common functionalities provided here, ensuring consistency and robustness
    * in the authentication layer. 
    * 
    * Generic parameters:
    * 'T' denotes the type of the service or component using these dependencies.
    * 'TApplicationUser' represents the type of the application user, typically tied with the identity system.
*/

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace SafeCommerce.Utilities.Dependencies;

/// <summary>
/// extends the base utility dependencies with additional authentication-specific dependencies
/// </summary>
/// <typeparam name="T">The type of the class utilizing this utility.</typeparam>
/// <typeparam name="TApplicationUser">The type of the class utilizing this utility.</typeparam>
public class Util_BaseAuthDependencies<T, TApplicationUser, TContext> : Util_BaseContextDependencies<TContext, T>
where T : class
where TApplicationUser : class
where TContext : class
{

    /// <summary>
    /// Manager to handle user-related operations.
    /// </summary>
    protected readonly UserManager<TApplicationUser> _userManager;
    /// <summary>
    /// Gets the configurations.
    /// </summary>
    protected readonly IConfiguration _configuration;
    /// <summary>
    /// Initializes a new instance of the <see cref="Util_BaseAuthDependencies{T, TApplicationUser}"/> class.
    /// </summary>
    /// <param name="mapper">An instance of IMapper to handle object mappings.</param>
    /// <param name="logger">An instance of ILogger specific to the type T.</param>
    /// <param name="configuration">The configurations</param>
    /// <param name="httpContextAccessor">An instance of IHttpContextAccessor to access current HTTP context.</param>
    /// <param name="userManager">Manager to handle user-related operations specific to TApplicationUser.</param>
    public Util_BaseAuthDependencies
    (
        IMapper mapper,
        ILogger<T> logger,
        IHttpContextAccessor httpContextAccessor,
        UserManager<TApplicationUser> userManager,
        IConfiguration configuration,
        TContext _db
    )
    : base
    (
        _db,
        mapper,
        logger,
        httpContextAccessor
    )
    {
        _userManager = userManager;
        _configuration = configuration;
    }
}