using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SafeCommerce.MediatR.Actions.Commands.UserManagment;

public class MediatR_UploadProfilePictureCommand
(
    Guid userId, 
    IFormFile? image
) : IRequest<ObjectResult>
{
    public Guid UserId { get; set; } = userId;

    public IFormFile? Image { get; set; } = image;
}