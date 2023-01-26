using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace Theatr.DAL.Entities
{
    public class Performance
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}