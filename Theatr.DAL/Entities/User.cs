using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Theatr.DAL.Entities
{
    public class User : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
