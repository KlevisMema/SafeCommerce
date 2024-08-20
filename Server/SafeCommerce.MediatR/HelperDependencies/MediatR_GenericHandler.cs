/* 
 * Defines a generic MediatR handler class that provides common functionalities for handling MediatR requests.
*/

namespace SafeCommerce.MediatR.Dependencies;

/// <summary>
/// Represents a generic MediatR handler that provides common functionalities and dependency injection for service classes.
/// </summary>
/// <typeparam name="TService">The type of the service class that the handler depends on.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_GenericHandler{TService}"/> class with the specified service.
/// </remarks>
/// <param name="service">The service instance to be used by the handler.</param>
public class MediatR_GenericHandler<TService>(TService service)
{
    /// <summary>
    /// Gets the service instance that the handler uses to process requests.
    /// </summary>
    protected readonly TService _service = service;
}