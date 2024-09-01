using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.HandleResponses;
using Talabat.Core.Entities;
using Talabat.services.Services.OrderService;
using Talabat.services.Services.OrderService.Dto;

namespace Talabat.APIS.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<ActionResult<OrderResultDto>> CreateOrderAsync(OrderDto orderDto)
        {
            var order = await _orderService.CreateOrderAsync(orderDto);
            if (order is null)
                return BadRequest(new ApiResponse(400, "Error while creating your Order"));
            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResultDto>>> GetAllOrdersForUserAsync()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetAllOrdersForUserAsync(email);

            if (orders is { Count: <= 0 })
                return Ok(new ApiResponse(200, "you don't have any Orders yet"));

            return Ok(orders);
        }
        [HttpGet]
        public async Task<ActionResult<OrderResultDto>> GetOrderByIdAsync(int id)
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order is null)
                return Ok(new ApiResponse(200, $"There is no Order with Id {id}"));

            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethods()
            => Ok(await _orderService.GetAllDeliveryMethods());
    }
}
