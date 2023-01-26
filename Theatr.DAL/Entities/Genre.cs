using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Theatr.DAL.Entities
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Performance> Perfomances { get; set; }

    }
}