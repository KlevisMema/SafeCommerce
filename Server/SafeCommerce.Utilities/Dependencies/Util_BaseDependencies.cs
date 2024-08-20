/*
     * This utility class provides a foundational structure for services by 
     * encapsulating common dependencies such as logging, mapping, and HTTP context.
     * The generic type parameter 'T' denotes the type of the service or component
     * using these base dependencies. By providing this common structure, services 
     * can maintain a consistent architecture, ensuring each service has access to 
     * essential functionalities.
*/

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SafeCommerce.Utilities.Dependencies;

/// <summary>
/// Represents the base dependencies required for utility services.
/// </summary>
/// <typeparam name="T">The type of the class utilizing this utility.</typeparam>
public class Util_BaseDependencies<T>
where T : class
{
    /// <summary>
    /// Provides a mapping capability to convert from one object type to another.
    /// </summary>
    protected readonly IMapper _mapper;
    /// <summary>
    /// Represents a type-specific logger.
    /// </summary>
    protected readonly ILogger<T> _logger;
    /// <summary>
    /// Provides access to the current HTTP context.
    /// </summary>
    protected readonly IHttpContextAccessor _httpContextAccessor;
    /// <summary>
    /// Initializes a new instance of the <see cref="Util_BaseDependencies{T}"/> class.
    /// </summary>
    /// <param name="mapper">An instance of IMapper to handle object mappings.</param>
    /// <param name="logger">An instance of ILogger specific to the type T.</param>
    /// <param name="httpContextAccessor">An instance of IHttpContextAccessor to access current HTTP context.</param>
    public Util_BaseDependencies
    (
        IMapper mapper,
        ILogger<T> logger,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
}