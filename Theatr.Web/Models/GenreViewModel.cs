using System.Collections.Generic;
using Theatr.Web.Models;

namespace Theatr.Web.Models
{
    public class GenreViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PerformanceViewModel> Performances { get; set; }
    }
}
