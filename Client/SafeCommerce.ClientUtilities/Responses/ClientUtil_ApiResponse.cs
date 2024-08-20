using System.Net;

namespace SafeCommerce.ClientUtilities.Responses;

public class ClientUtil_ApiResponse<T>
{
    /// <summary>
    ///     A <see cref="string"/> custom message describing an action succsess or fail  etc. 
    ///     It's nullable.
    /// </summary>
    public string? Message { get; set; }
    /// <summary>
    ///     A <see cref="bool"/>, indicating if an operation was succsess or not.
    /// </summary>
    public bool Succsess { get; set; }
    /// <summary>
    ///     A <see cref="HttpStatusCode"/>, indicatig the operation status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }
    /// <summary>
    ///     A <see cref="T"/> obejct of any type.
    /// </summary>
    public T? Value { get; set; }
    /// <summary>
    ///     A <see cref="List{T}"/> of string indicating errors.
    /// </summary>
    public List<string>? Errors { get; set; }
}