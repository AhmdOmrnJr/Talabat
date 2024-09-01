using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Infrastructure.BasketRepository.BasketEntities;

namespace Talabat.services.Services.BasketService.DTO
{
    public class BasketProfiler: Profile
    {
        public BasketProfiler()
        {
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
        }
    }
}
