using MediatR;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.MediatR.Actions.Queries.Authentication;

public class MediatR_VerifyPkQuery
(
    Guid userId,
    string publicKey
) : IRequest<Util_GenericResponse<bool>>
{
    public Guid UserId { get; set; } = userId;
    public string PublicKey { get; set; } = publicKey;
}