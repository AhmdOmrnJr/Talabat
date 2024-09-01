using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.services.Services.ProductSercice.DTO
{
    public class ProductProfiler: Profile
    {
        public ProductProfiler()
        {
            CreateMap<Product, ProductResultDTO>()
                .ForMember(dest => dest.ProductBrandName, options => options.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductTypeName, options => options.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom<ProductUrlResolver>());
        }
    }
}
