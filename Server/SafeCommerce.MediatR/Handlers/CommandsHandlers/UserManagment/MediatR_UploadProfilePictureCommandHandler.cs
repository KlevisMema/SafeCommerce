using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.UserManagment.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.UserManagment;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.UserManagment;

public class MediatR_UploadProfilePictureCommandHandler(
    IAccountManagment service
) : MediatR_GenericHandler<IAccountManagment>(service),
    IRequestHandler<MediatR_UploadProfilePictureCommand, ObjectResult>
{
    public async Task<ObjectResult> 
    Handle
    (
        MediatR_UploadProfilePictureCommand request, 
        CancellationToken cancellationToken
    )
    {
        var uploadProfilePicResult = await _service.UploadProfilePicture(request.UserId, request.Image);

        return Util_GenericControllerResponse<byte[]>.ControllerResponse(uploadProfilePicResult);
    }
}