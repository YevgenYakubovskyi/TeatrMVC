using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Theatr.DAL.Entities;

namespace Theatr.DAL.Identity
{
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> theatr)
                : base(theatr)
        {
        }
    }
}