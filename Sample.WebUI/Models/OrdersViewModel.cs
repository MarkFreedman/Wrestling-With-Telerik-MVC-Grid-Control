using System.Collections.Generic;
using System.Linq;
using System.Collections;
using SampleModel;

namespace Sample.WebUI.Models
{
    public class OrdersViewModel
    {
        public IQueryable<OrderViewModel> Orders { get; set; }

        public OrdersViewModel(int customerId)
        {
            var context = new SampleEntities();
            Orders = from o in context.Orders
                     orderby o.DatePlaced
                     where o.CustomerID == customerId
                     select
                         new OrderViewModel
                             {
                                 OrderId = o.ID,
                                 CustomerId = o.CustomerID,
                                 DatePlaced = o.DatePlaced,
                                 OrderSubtotal = o.OrderSubtotal,
                                 OrderTax = o.OrderTax,
                                 OrderTotal = o.OrderTotal,
                                 OrderChannelId = o.OrderChannel.ID,
                                 OrderChannelName = o.OrderChannel.Name
                             };
        }
    }
}