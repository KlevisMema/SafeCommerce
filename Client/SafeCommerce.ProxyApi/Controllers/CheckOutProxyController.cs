using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.DataTransormObject.CheckOut;
using SafeCommerce.ProxyApi.Container.Interfaces;

namespace SafeCommerce.ProxyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Default")]
public class CheckOutProxyController(ICheckOutProxyService proxyService) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpPost(Route_CheckOut.ProxyCheckOut)]
    public ActionResult<string>
    ChechOut
    (
        [FromRoute] string userId,
        [FromBody] List<DTO_CartItem> cartItems
    )
    {
        if (cartItems == null)
            return BadRequest("No items in the cart!");

        var session = proxyService.CreateCheckOutSesssion(cartItems);

        if (session is not null)
            return Ok(session.Url);

        return BadRequest("Session could not be created!");
    }
}