using System.Collections.Generic;

namespace Theatr.BLL.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<PerformanceDTO> PerfomanceDTOs { get; set; }
    }
}
