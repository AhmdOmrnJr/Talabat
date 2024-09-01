using Talabat.Core.Entities.OrderEntities;

namespace Talabat.Infrastructure.Specifications
{
    public class OrderWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsSpecification(string email) : base(order => order.BuyerEmail == email)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDesc(order => order.OrderDate);
        }
        public OrderWithItemsSpecification(int id, string email) : base(order => order.BuyerEmail == email && order.Id == id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
