
namespace Theatr.BLL.DTO
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public int PerfomanceId { get; set; }
        public PerformanceDTO Perfomance { get; set; }
        public string TicketCategory { get; set; }
        public float Price { get; set; }
        public string TicketState { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
    }
}
