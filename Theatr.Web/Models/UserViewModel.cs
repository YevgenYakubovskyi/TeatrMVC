using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Theatr.Web.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IEnumerable<TicketViewModel> Tickets { get; set; }

    }
}