using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderEntities;

namespace Talabat.Infrastructure.Specifications
{
    public class OrderWithPaymentIntentSpesification : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpesification(string paymentIntentId) : base(order => order.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
