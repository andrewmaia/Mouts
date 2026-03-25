using AutoMapper;
using Mouts.Application.UseCases.Common;
using Mouts.Domain.Entities;

namespace Mouts.Application.Mapping;

public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<Sale, SaleOutput>();
        CreateMap<SaleItem, SaleItemOutput>();
    }
}
