using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sample.WebUI.Models
{
    public class OrderViewModel
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public int CustomerId { get; set; }

        [Required]
        [DisplayName("Order Placed")]
        [DataType(DataType.Date)]
        public DateTime? DatePlaced { get; set; }

        [Required]
        [DisplayName("Subtotal")]
        public decimal? OrderSubtotal { get; set; }

        [Required]
        [DisplayName("Tax")]
        public decimal? OrderTax { get; set; }

        [Required]
        [DisplayName("Total")]
        public decimal? OrderTotal { get; set; }

        [ScaffoldColumn(false)]
        public int OrderChannelId { get; set; }

        [DisplayName("Channel")]
        [UIHint("OrderChannel")]
        public string OrderChannelName { get; set; }
    }
}