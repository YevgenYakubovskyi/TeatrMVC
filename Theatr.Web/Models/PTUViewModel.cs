using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatr.Web.Models;

namespace Theatr.Web.Models
{
    public class PTUViewModel
    {
        public IEnumerable<PerformanceViewModel> Performance { get; set; }
        public IEnumerable<TicketViewModel> Tickets { get; set; }
    }
}