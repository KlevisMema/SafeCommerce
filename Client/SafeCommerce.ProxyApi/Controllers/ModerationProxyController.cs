using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using Microsoft.AspNetCore.Authorization;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ProxyApi.Container.Interfaces;
using SafeCommerce.DataTransormObject.ModerationHistory;

namespace SafeCommerce.ProxyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Default")]
public class ModerationProxyController
(
    IModerationProxyService _moderationProxyService,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions
) : ControllerBase
{
    [Authorize(Roles = "Moderator")]
    [HttpGet(Route_Moderation.ProxyGetModerationHistoryForModerator)]
    public async Task<ActionResult<Util_GenericResponse<DTO_SplittedModerationHistory>>>
    GetModerationHistoryForModerator()
    {
        var result = await _moderationProxyService.GetModerationHistoryForModerator
        (
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<DTO_SplittedModerationHistory>.ControllerResponse(result);
    }
}