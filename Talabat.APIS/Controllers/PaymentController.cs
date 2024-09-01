using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIS.HandleResponses;
using Talabat.services.Services.BasketService.DTO;
using Talabat.services.Services.OrderService.Dto;
using Talabat.services.Services.PaymentService;

namespace Talabat.APIS.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private const string whSecret = " whsec_a8cac15484c6fd6cbe10d0ceb5b9070a6285bd435c82c88d24ddfd23e95975ac";

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("basketId")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateCutomerPaymentIntentForExistingOrder(CustomerBasketDto basket)
        {
            var customerBasket = await _paymentService.CreateOrUpdateCutomerPaymentIntentForExistingOrder(basket);

            if (customerBasket == null)
                return BadRequest(new ApiResponse(400, "Problem with your Basket"));

            return Ok(customerBasket);
        }
        [HttpPost("basketId")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateCutomerPaymentIntentForNewOrder(string basketId)
        {
            var customerBasket = await _paymentService.CreateOrUpdateCutomerPaymentIntentForNewOrder(basketId);

            if (customerBasket == null)
                return BadRequest(new ApiResponse(400, "Problem with your Basket"));

            return Ok(customerBasket);
        }

        [HttpPost]
        public async Task<ActionResult> WebHook(string basketId)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], whSecret);

                PaymentIntent paymentIntent;
                OrderResultDto order;

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded: ", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id, basketId);
                    _logger.LogInformation("Order updated to Payment Succeed", order.Id);
                }
                else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed: ", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("Order updated to Payment Failed", order.Id);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }

        }


    }
}
