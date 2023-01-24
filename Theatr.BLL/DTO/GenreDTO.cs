using System.Collections.Generic;

namespace Theatr.BLL.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<PerformanceDTO> Perfomances { get; set; }
    }
}
