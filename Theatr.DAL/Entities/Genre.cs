using System.Collections.Generic;

namespace Theatr.DAL.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Performance> Perfomances { get; set; }

    }
}