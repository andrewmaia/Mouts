using AutoMapper;
using Mouts.Api.Contracts.Sales;
using Mouts.Application.UseCases.Common;
using Mouts.Application.UseCases.CreateSale;
using Mouts.Application.UseCases.UpdateSale;

namespace Mouts.Api.Mapping;

public class SalesApiMappingProfile : Profile
{
    public SalesApiMappingProfile()
    {
        CreateMap<SaleItemApiRequest, SaleItemInput>();
        CreateMap<CreateSaleApiRequest, CreateSaleRequest>();
        CreateMap<UpdateSaleApiRequest, UpdateSaleRequest>();
    }
}
