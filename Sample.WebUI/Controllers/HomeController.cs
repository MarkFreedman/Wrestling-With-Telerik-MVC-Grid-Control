using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sample.WebUI.Infrastructure;
using Sample.WebUI.Models;
using SampleModel;
using Telerik.Web.Mvc;
using System;

namespace Sample.WebUI.Controllers
{
    public class HomeController : Controller
    {
        #region Core Action Methods

        public ActionResult Index()
        {
            return View();
        }
        
        #endregion

        #region AJAX Master Grid Action Methods
        
        [GridAction]
        public ActionResult AjaxCustomersHierarchy()
        {
            return View(new GridModel(GetCustomers()));
        }

        [GridAction]
        public ActionResult AjaxSaveCustomer(int customerId, string lastName, string firstName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var context = new SampleEntities();
                    var customer = context.Customers.Single(c => c.ID == customerId);
                    UpdateModel(customer);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Response.StatusCode = 500;
                    Response.AppendHeader("message",
                                          "There was an issue editing data for customer \"" + firstName + " " + lastName +
                                          "\". Please contact tech support with this message: " + e.Message);
                }
            }

            return View(new GridModel(GetCustomers()));
        }

        [GridAction]
        public ActionResult AjaxAddCustomer(string lastName, string firstName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var context = new SampleEntities();
                    var customer = new Customer();
                    UpdateModel(customer);
                    context.Customers.AddObject(customer);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Response.StatusCode = 500;
                    Response.AppendHeader("message",
                                          "There was an issue adding customer \"" + firstName + " " + lastName +
                                          "\". Please contact tech support with this message: " + Utility.GetInnermostException(e).Message);
                }
            }

            return View(new GridModel(GetCustomers()));
        }

        [GridAction]
        public ActionResult AjaxDeleteCustomer(int customerId)
        {
            try
            {
                var context = new SampleEntities();
                var customer = context.Customers.Single(c => c.ID == customerId);
                bool orders = customer.Orders.Any();

                if (orders)
                {
                    Response.StatusCode = 500;
                    Response.AppendHeader("message", "This customer has order history, and cannot be deleted until all orders are deleted.");
                }
                else
                {
                    context.Customers.DeleteObject(customer);
                    context.SaveChanges();
                }
            }
            catch (System.Exception e)
            {
                Response.StatusCode = 500;
                Response.AppendHeader("message", "There was an issue deleting this customer. Please contact tech support with this message: " + Utility.GetInnermostException(e).Message);
            }

            return View(new GridModel(GetCustomers()));
        }

        #endregion

        #region AJAX Detail Grid Action Methods

        [GridAction]
        public ActionResult AjaxOrdersForCustomerHierarchy(int customerId)
        {
            return View(new GridModel(GetOrders(customerId)));
        }

        [GridAction]
        public ActionResult AjaxSaveOrder(int customerId, int orderId)
        {
            if (ModelState.IsValid)
            {
                var context = new SampleEntities();
                var order = context.Customers.Single(c => c.ID == customerId).Orders.Single(o => o.ID == orderId);

                try
                {
                    UpdateModel(order);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Response.StatusCode = 500;
                    Response.AppendHeader("message",
                                          "There was an issue editing data for this order. Please contact tech support with this message: " + e.Message);
                }
            }

            return View(new GridModel(GetOrders(customerId)));
        }

        [GridAction]
        public ActionResult AjaxAddOrder(int customerId)
        {
            if (ModelState.IsValid)
            {
                var context = new SampleEntities();
                var customer = context.Customers.Single(c => c.ID == customerId);
                var order = new Order();

                try
                {
                    UpdateModel(order);
                    customer.Orders.Add(order);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Response.StatusCode = 500;
                    Response.AppendHeader("message",
                                          "There was an issue adding order to customer \"" + customer.FirstName + " " + customer.LastName +
                                          "\". Please contact tech support with this message: " + Utility.GetInnermostException(e).Message);
                }
            }

            return View(new GridModel(GetOrders(customerId)));
        }

        [GridAction]
        public ActionResult AjaxDeleteOrder(int customerId, int orderId)
        {
            try
            {
                var context = new SampleEntities();
                var customer = context.Customers.Single(c => c.ID == customerId);
                var order = customer.Orders.Single(o => o.ID == orderId);
                context.DeleteObject(order);
                context.SaveChanges();
            }
            catch (System.Exception e)
            {
                Response.StatusCode = 500;
                Response.AppendHeader("message", "There was an issue deleting order " + orderId + ". Please contact tech support with this message: " + Utility.GetInnermostException(e).Message);
            }

            return View(new GridModel(GetOrders(customerId)));
        }

        #endregion

        #region AJAX Support Methods

        public JsonResult CheckDuplicateCustomerName(CustomerViewModel model)
        {
            return Json(ValidateCustomer(model), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrderChannels()
        {
            var viewModel = new ChannelsViewModel();
            var channels = from c in viewModel.Channels select c;
            return Json(channels, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        private IEnumerable<CustomerViewModel> GetCustomers()
        {
            var viewModel = new CustomersViewModel();
            var customers = from c in viewModel.Customers select c;
            return customers.ToList();
        }

        private IEnumerable<OrderViewModel> GetOrders(int customerId)
        {
            var viewModel = new OrdersViewModel(customerId);
            var orders = from o in viewModel.Orders select o;
            return orders.ToList();
        }

        private bool ValidateCustomer(CustomerViewModel model)
        {
            var context = new SampleEntities();

            return !context.Customers.Any(c => c.LastName.ToLower() == model.LastName.ToLower() &&
                                               c.FirstName.ToLower() == model.FirstName.ToLower() &&
                                               (model.MiddleName == null ? c.MiddleName == null : c.MiddleName.ToLower() == model.MiddleName.ToLower()) &&
                                               c.ID != model.CustomerId);
        }

        #endregion
    }
}
