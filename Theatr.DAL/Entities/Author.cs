using System.Collections.Generic;

namespace Theatr.DAL.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Performance> Perfomances { get; set; }
    }
}