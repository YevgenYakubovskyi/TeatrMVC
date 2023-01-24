using System.Collections.Generic;

namespace Theatr.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public virtual IEnumerable<TicketDTO> Tickets { get; set; }
    }
}
