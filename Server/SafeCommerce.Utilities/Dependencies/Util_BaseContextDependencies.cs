/* 
 * Provides a base class with common dependencies for repositories utilizing a database context.
 * This class is part of the Utilities layer and helps in reducing boilerplate code by encapsulating 
 * common functionalities.
 */

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SafeCommerce.Utilities.Dependencies;

/// <summary>
/// Provides a base class with common dependencies for repositories that require access to a database context.
/// It includes necessary components like IMapper, ILogger, and IHttpContextAccessor, along with the database context.
/// </summary>
/// <typeparam name="TContext">The type of the database context.</typeparam>
/// <typeparam name="TRepository">The type of the repository.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="Util_BaseContextDependencies{TContext, TRepository}"/> class.
/// </remarks>
/// <param name="mapper">An instance of IMapper to handle object mappings.</param>
/// <param name="logger">An instance of ILogger specific to the type T.</param>
/// <param name="db">An instance of Application db context</param>
/// <param name="httpContextAccessor">An instance of IHttpContextAccessor to access current HTTP context.</param>
public class Util_BaseContextDependencies<TContext, TRepository>
(
    TContext db,
    IMapper mapper,
    ILogger<TRepository> logger,
    IHttpContextAccessor httpContextAccessor
) : Util_BaseDependencies<TRepository>
    (
        mapper,
        logger,
        httpContextAccessor
    )
where TRepository : class
where TContext : class
{
    /// <summary>
    /// Represents the database context
    /// </summary>
    protected readonly TContext _db = db;
}