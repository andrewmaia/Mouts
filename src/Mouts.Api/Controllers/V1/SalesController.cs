using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mouts.Api.Contracts;
using Mouts.Api.Contracts.Sales;
using Mouts.Api.Extensions;
using Mouts.Application.Common;
using Mouts.Application.Execution;
using Mouts.Application.UseCases.CancelSale;
using Mouts.Application.UseCases.CancelSaleItem;
using Mouts.Application.UseCases.CreateSale;
using Mouts.Application.UseCases.GetSaleById;
using Mouts.Application.UseCases.GetSales;
using Mouts.Application.UseCases.UpdateSale;
using Mouts.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace Mouts.Api.Controllers.V1;

[ApiController]
[Route("api/sales")]
public class SalesController : ControllerBase
{
    private readonly RequestExecutor _executor;
    private readonly IMapper _mapper;

    public SalesController(RequestExecutor executor, IMapper mapper)
    {
        _executor = executor;
        _mapper = mapper;
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Sale created successfully", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ApiResponse))]
    public async Task<IActionResult> Create([FromBody] CreateSaleApiRequest apiRequest)
    {
        var request = _mapper.Map<CreateSaleRequest>(apiRequest);

        var result = await _executor.ExecuteAsync<CreateSaleRequest, CreateSaleResponse>(request);

        if (!result.IsSuccess)
            return HandleError(result);

        return Ok(result.ToApiResponse());
    }

    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, "Sales returned successfully", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query", typeof(ApiResponse))]
    public async Task<IActionResult> GetAll(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int size = 10,
        [FromQuery(Name = "_order")] string? order = null,
        [FromQuery] string? saleNumber = null,
        [FromQuery] string? customerName = null,
        [FromQuery] string? branchName = null,
        [FromQuery] SaleStatus? status = null,
        [FromQuery(Name = "saleDate_min")] DateTime? saleDateMin = null,
        [FromQuery(Name = "saleDate_max")] DateTime? saleDateMax = null,
        [FromQuery(Name = "totalAmount_min")] decimal? totalAmountMin = null,
        [FromQuery(Name = "totalAmount_max")] decimal? totalAmountMax = null)
    {
        var request = new GetSalesRequest
        {
            Page = 0,
            Size = 10000,
            Order = order,
            SaleNumber = saleNumber,
            CustomerName = customerName,
            BranchName = branchName,
            Status = status,
            SaleDateMin = saleDateMin,
            SaleDateMax = saleDateMax,
            TotalAmountMin = totalAmountMin,
            TotalAmountMax = totalAmountMax
        };

        var result = await _executor.ExecuteAsync<GetSalesRequest, GetSalesResponse>(request);

        if (!result.IsSuccess)
            return HandleError(result);

        return Ok(result.ToApiResponse());
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sale returned successfully", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sale not found", typeof(ApiResponse))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _executor.ExecuteAsync<GetSaleByIdRequest, GetSaleByIdResponse>(
            new GetSaleByIdRequest { SaleId = id });

        if (!result.IsSuccess)
            return HandleError(result);

        return Ok(result.ToApiResponse());
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sale updated successfully", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sale not found", typeof(ApiResponse))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleApiRequest apiRequest)
    {
        var request = _mapper.Map<UpdateSaleRequest>(apiRequest);
        request.SaleId = id;

        var result = await _executor.ExecuteAsync<UpdateSaleRequest, UpdateSaleResponse>(request);

        if (!result.IsSuccess)
            return HandleError(result);

        return Ok(result.ToApiResponse());
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sale cancelled successfully", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sale not found", typeof(ApiResponse))]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _executor.ExecuteAsync<CancelSaleRequest, CancelSaleResponse>(
            new CancelSaleRequest { SaleId = id });

        return Ok(result.ToApiResponse());
    }

    [HttpPost("{id:guid}/cancel")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sale cancelled successfully", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sale not found", typeof(ApiResponse))]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var result = await _executor.ExecuteAsync<CancelSaleRequest, CancelSaleResponse>(
            new CancelSaleRequest { SaleId = id });

        if (!result.IsSuccess)
            return HandleError(result);

        return Ok(result.ToApiResponse());
    }

    [HttpPost("{id:guid}/items/{itemId:guid}/cancel")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sale item cancelled successfully", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sale or item not found", typeof(ApiResponse))]
    public async Task<IActionResult> CancelItem(Guid id, Guid itemId)
    {
        var result = await _executor.ExecuteAsync<CancelSaleItemRequest, CancelSaleItemResponse>(
            new CancelSaleItemRequest
            {
                SaleId = itemId,
                ItemId = id
            });

        if (!result.IsSuccess)
            return HandleError(result);

        return Ok(result.ToApiResponse());
    }

    private IActionResult HandleError(ResultResponse result)
    {
        return result.BusinessError switch
        {
            BusinessError.NotFound => NotFound(result.ToApiResponse()),
            BusinessError.Conflict => Conflict(result.ToApiResponse()),
            _ => BadRequest(result.ToApiResponse())
        };
    }
}
