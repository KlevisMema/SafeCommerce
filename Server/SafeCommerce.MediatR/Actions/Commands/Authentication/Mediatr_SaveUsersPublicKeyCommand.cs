using MediatR;
using SafeCommerce.DataTransormObject.UserManagment;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.MediatR.Actions.Commands.Authentication;

public class Mediatr_SaveUsersPublicKeyCommand
(
    Guid userId,
    DTO_SavePublicKey publicKey
) : IRequest<Util_GenericResponse<string>>
{
    public Guid UserId { get; set; } = userId;
    public DTO_SavePublicKey PublicKey { get; set; } = publicKey;
}