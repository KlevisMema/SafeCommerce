/*
 * Utility class for generating standardized responses for controllers.
 * Provides a convenient way to return standard HTTP responses based on the service response's status code.
*/

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SafeCommerce.Utilities.Responses;

/// <summary>
///     A class that provides statuss code response to be used in controllers.
///     Provides better code readablility in the controller part.
/// </summary>
public class Util_GenericControllerResponse<T> : ControllerBase
{
    /// <summary>
    ///     Return a object result with <see cref="Util_GenericResponse{T}"/> as object parameter depending from the service <see cref="HttpStatusCode"/> response.
    /// </summary>
    /// <param name="obj"> A <see cref="Util_GenericResponse{T}"/> object </param>
    /// <returns> <see cref="ObjectResult"/> </returns>
    public static ObjectResult ControllerResponse(Util_GenericResponse<T> obj)
    {
        return obj.StatusCode switch
        {
            HttpStatusCode.NotFound => new NotFoundObjectResult(obj),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(obj),
            HttpStatusCode.OK => new OkObjectResult(obj),
            HttpStatusCode.Forbidden => new BadRequestObjectResult(obj),
            _ => new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError },
        };
    }
    /// <summary>
    ///     Return a object result with <see cref="Util_GenericResponse{T}"/> as object parameter depending from the service <see cref="HttpStatusCode"/> response.
    /// </summary>
    /// <param name="obj">List of object that will come from a controller</param>
    /// <returns>The appropriate status code</returns>
    public static ObjectResult ControllerResponseList(Util_GenericResponse<List<T>> obj)
    {
        return obj.StatusCode switch
        {
            HttpStatusCode.NotFound => new NotFoundObjectResult(obj),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(obj),
            HttpStatusCode.OK => new OkObjectResult(obj),
            HttpStatusCode.Forbidden => new BadRequestObjectResult(obj),
            _ => new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError },
        };
    }
}