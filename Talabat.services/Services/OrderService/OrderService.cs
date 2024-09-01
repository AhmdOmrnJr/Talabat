using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderEntities;
using Talabat.Infrastructure.Interfaces;
using Talabat.Infrastructure.Specifications;
using Talabat.services.Services.BasketService;
using Talabat.services.Services.BasketService.DTO;
using Talabat.services.Services.OrderService.Dto;
using Talabat.services.Services.PaymentService;

namespace Talabat.services.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<OrderResultDto> CreateOrderAsync(OrderDto orderDto)
        {
            var basket = await _basketService.GetBasketAsync(orderDto.BasketId);
            if (basket is null)
                return null;

            var orderItems = new List<OrderItemDto>();

            foreach (var item in basket.BasketItems)
            {
                var ProductItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(ProductItem.Id, ProductItem.Name, ProductItem.PictureUrl);
                var orderItem = new OrderItem(ProductItem.Price, item.Quantity, itemOrdered);
                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);
                orderItems.Add(mappedOrderItem);
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var specs = new OrderWithPaymentIntentSpesification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecifictionAsync(specs);

            CustomerBasketDto customerBasket = new CustomerBasketDto();

            if (existingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdateCutomerPaymentIntentForExistingOrder(basket);
            }
            else
            {
                customerBasket = await _paymentService.CreateOrUpdateCutomerPaymentIntentForNewOrder(basket.Id);
            }

            var mappedShippingAddress = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order(orderDto.BuyerEmail, mappedShippingAddress, deliveryMethod, mappedOrderItems, subTotal, basket.PaymentIntentId ?? customerBasket.PaymentIntentId);

            await _unitOfWork.Repository<Order>().Add(order);

            await _unitOfWork.Complete();

            //await _basketService.DeleteBasketAsync(orderDto.BasketId); ==> wrong line Delete Does not happen here

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethods()
            => _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecifictionAsync(specs);
            var mappedOrders = _mapper.Map<List<OrderResultDto>>(orders);
            return mappedOrders;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(id, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecifictionAsync(specs);
            var mappedOrder = _mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }
    }
}
