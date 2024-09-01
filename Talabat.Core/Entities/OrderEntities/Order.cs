namespace Talabat.Core.Entities.OrderEntities
{
    public enum OrderStatus
    {
        Pending,
        PaymentRecieved,
        PaymentFailed
    }

    public class Order: BaseEntity
    {

        public Order()
        {
            
        }

        public Order(string buyeremail, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, IReadOnlyList<OrderItem> orderItems, decimal subTotal, string? paymentIntentId)
        {
            BuyerEmail = buyeremail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Price;
    }
}
