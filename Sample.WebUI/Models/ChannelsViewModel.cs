using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web.Mvc;
using SampleModel;

namespace Sample.WebUI.Models
{
    public class ChannelsViewModel
    {
        public IList<SelectListItem> Channels { get; set; }

        public ChannelsViewModel()
        {
            var context = new SampleEntities();

            var channels = context.OrderChannels
                   .OrderBy(c => c.Name)
                   .Select(c =>
                           new SelectListItem
                           {
                               Selected = false,
                               Text = c.Name,
                               Value = SqlFunctions.StringConvert((double)c.ID)
                           });

            Channels = channels.ToList();
        }
    }
}