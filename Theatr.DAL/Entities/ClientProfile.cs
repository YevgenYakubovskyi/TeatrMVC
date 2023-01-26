using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatr.DAL.Entities
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }

        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
