using System;
using System.Collections.Generic;
using Theatr.Web.Models;

namespace Theatr.Web.Models
{
    public class PerformanceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<AuthorViewModel> Authors { get; set; }
        public IEnumerable<GenreViewModel> Genres { get; set; }
        public IEnumerable<TicketViewModel> Tickets { get; set; }
    }
}
