using AutoMapper;
using Warehouse_API.DTO;
using Warehouse_API.Models;

namespace Warehouse_API.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDTO>();            CreateMap<ProductDTO, Product>();
            CreateMap<Warehouse, WarehouseDTO>();        CreateMap<WarehouseDTO, Warehouse>();
            CreateMap<Stock, StockDTO>();        CreateMap<StockDTO, Stock>();

        }
    }
}
