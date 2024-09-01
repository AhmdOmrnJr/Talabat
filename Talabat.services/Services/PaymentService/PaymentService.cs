using AutoMapper;
using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderEntities;
using Talabat.Infrastructure.Interfaces;
using Talabat.Infrastructure.Specifications;
using Talabat.services.Services.BasketService;
using Talabat.services.Services.BasketService.DTO;
using Talabat.services.Services.OrderService.Dto;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.services.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IBasketService basketService, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<CustomerBasketDto> CreateOrUpdateCutomerPaymentIntentForExistingOrder(CustomerBasketDto basket)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            if (basket == null)
                return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != productItem.Price)
                    item.Price = productItem.Price;
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + ((long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + ((long)shippingPrice * 100),
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketService.UpdateBasketAsync(basket);

            return basket;
        }
        public async Task<CustomerBasketDto> CreateOrUpdateCutomerPaymentIntentForNewOrder(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var basket = await _basketService.GetBasketAsync(basketId);

            if (basket == null)
                return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != productItem.Price)
                    item.Price = productItem.Price;
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + ((long)shippingPrice * 100),
                    Currency ="usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + ((long)shippingPrice * 100),
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketService.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<OrderResultDto> UpdateOrderPaymentFailed(string PaymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpesification(PaymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecifictionAsync(specs);

            if (order == null)
                return null;

            order.OrderStatus = OrderStatus.PaymentFailed;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();

            var mappedOrder = _mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }

        public async Task<OrderResultDto> UpdateOrderPaymentSucceeded(string PaymentIntentId, string basketId)
        {
            var specs = new OrderWithPaymentIntentSpesification(PaymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecifictionAsync(specs);

            if (order == null)
                return null;

            order.OrderStatus = OrderStatus.PaymentRecieved;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();

            await _basketService.DeleteBasketAsync(basketId);

            var mappedOrder = _mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }
    }
}
