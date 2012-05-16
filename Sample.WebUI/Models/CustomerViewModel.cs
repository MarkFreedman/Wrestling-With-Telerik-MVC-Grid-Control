using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace Sample.WebUI.Models
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        [Required]
        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [Required]
        [Remote("CheckDuplicateCustomerName", 
                "Home", 
                AdditionalFields = "CustomerId, FirstName, MiddleName", 
                ErrorMessage = "This name has already been used for a customer. Please choose another name.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [Remote("CheckDuplicateCustomerName", 
                "Home", 
                AdditionalFields = "CustomerId, LastName, MiddleName", 
                ErrorMessage = "This name has already been used for a customer. Please choose another name.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        [Remote("CheckDuplicateCustomerName", 
                "Home", 
                AdditionalFields = "CustomerId, LastName, FirstName", 
                ErrorMessage = "This name has already been used for a customer. Please choose another name.")]
        public string MiddleName { get; set; }

        [DisplayName("Middle Initial")]
        public string MiddleInitial { get; set; }
    }
}

