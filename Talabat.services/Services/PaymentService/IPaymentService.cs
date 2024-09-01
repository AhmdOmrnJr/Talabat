using Talabat.services.Services.BasketService.DTO;
using Talabat.services.Services.OrderService.Dto;

namespace Talabat.services.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdateCutomerPaymentIntentForExistingOrder(CustomerBasketDto basket);
        Task<CustomerBasketDto> CreateOrUpdateCutomerPaymentIntentForNewOrder(string basketId);
        Task<OrderResultDto> UpdateOrderPaymentSucceeded(string PaymentIntentId, string basketId);
        Task<OrderResultDto> UpdateOrderPaymentFailed(string PaymentIntentId);
    }
}
