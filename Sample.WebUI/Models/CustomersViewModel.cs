using System.Collections.Generic;
using System.Linq;
using System.Collections;
using SampleModel;

namespace Sample.WebUI.Models
{
    public class CustomersViewModel
    {
        public IQueryable<CustomerViewModel> Customers { get; set; }

        public CustomersViewModel()
        {
            var context = new SampleEntities();
            Customers = from c in context.Customers
                        orderby c.LastName, c.FirstName
                        select
                            new CustomerViewModel
                                {
                                    CustomerId = c.ID,
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    MiddleName = c.MiddleName,
                                    MiddleInitial = c.MiddleName == "" ? "" : " " + c.MiddleName.Substring(0, 1) + ".",
                                    AccountNumber = c.AccountNumber
                                };
        }
    }
}