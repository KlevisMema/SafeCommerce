﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Shop;
using SafeCommerce.MediatR.Actions.Queries.Shop;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Shop;

public class MediatR_GetShopsQueryHandler
(
    IShopService service
) : MediatR_GenericHandler<IShopService>(service),
    IRequestHandler<MediatR_GetShopsQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (

        MediatR_GetShopsQuery request,
        CancellationToken cancellationToken
    )
    {
        var shops = await _service.GetPublicShops(request.UserId, cancellationToken);

        return Util_GenericControllerResponse<IEnumerable<DTO_Shop>>.ControllerResponse(shops);
    }
}