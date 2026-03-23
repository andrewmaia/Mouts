using Microsoft.AspNetCore.Mvc;
using Mouts.Api.Contracts;
using Mouts.Api.Contracts.Orders;
using Mouts.Api.Extensions;
using Mouts.Application.Common;
using Mouts.Application.Execution;
using Mouts.Application.UseCases.CreateOrder;
using Mouts.Application.UseCases.PayOrder;
using Swashbuckle.AspNetCore.Annotations;

namespace Mouts.Api.Controllers.V1;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly RequestExecutor _executor;

    public OrdersController(RequestExecutor executor)
    {
        _executor = executor;
    }

    [HttpPost]
    [SwaggerOperation(
    Summary = "Create a new order",
    Description = "Creates an order and returns a generic response wrapper.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Order created successfully",typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ApiResponse))]
    public async Task<IActionResult> Create([FromBody] CreateOrderApiRequest apiRequest)
    {
        var request = new CreateOrderRequest(apiRequest.TotalAmount);
        var result = await _executor.ExecuteAsync<CreateOrderRequest, CreateOrderResponse> (request);

        if (!result.IsSuccess)
            return BadRequest(result.ToApiResponse());

        return Created(string.Empty, result.ToApiResponse());
    }

    [HttpPost("Pay")]
    [SwaggerOperation(
    Summary = "Pay an order",
    Description = "Pay an order and returns a generic response wrapper.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Order paid successfully", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ApiResponse))]
    public async Task<IActionResult> PayOrder([FromBody] PayOrderApiRequest apiRequest)
    {
        var request = new PayOrderRequest(apiRequest.OrderId);
        var result = await _executor.ExecuteAsync<PayOrderRequest, PayOrderResponse>(request);

        if (!result.IsSuccess)
        {
            return result.BusinessError switch
            {
                BusinessError.NotFound => NotFound(result.ToApiResponse()),
                _ => BadRequest(result.ToApiResponse())
            };
        }

        return Ok(result.ToApiResponse());
    }

}                                                                                                                   
