using System;
using System.Collections.Generic;

namespace Theatr.BLL.DTO
{
    public class PerformanceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<AuthorDTO> Authors { get; set; }
        public virtual IEnumerable<GenreDTO> Genres { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual IEnumerable<TicketDTO> Tickets { get; set; }
    }
}
