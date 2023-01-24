
namespace Theatr.DAL.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int PerfomanceId { get; set; }
        public Performance Perfomance { get; set; }
        public string TicketCategory { get; set; }
        public float Price { get; set; }
        public string TicketState { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}