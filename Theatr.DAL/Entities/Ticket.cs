
using System.ComponentModel.DataAnnotations;

namespace Theatr.DAL.Entities
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public int PerfomanceId { get; set; }
        public Performance Perfomance { get; set; }
        public string TicketCategory { get; set; }
        public float Price { get; set; }
        public string TicketState { get; set; }
        public string UserId { get; set; }
        public ClientProfile User { get; set; }
    }
}