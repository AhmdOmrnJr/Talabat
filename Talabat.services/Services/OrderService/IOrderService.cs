using Talabat.Core.Entities;
using Talabat.services.Services.OrderService.Dto;

namespace Talabat.services.Services.OrderService
{
    public interface IOrderService
    {
        Task<OrderResultDto> CreateOrderAsync(OrderDto orderDto);
        Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail);
        Task<OrderResultDto> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>>  GetAllDeliveryMethods();
    }
}
