namespace Theatr.Web.Models
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public int PerfomanceId { get; set; }
        public PerformanceViewModel Performance { get; set; }
        public string TicketCategory { get; set; }
        public float Price { get; set; }
        public string TicketState { get; set; }
        public string UserId { get; set; }
    }
}
