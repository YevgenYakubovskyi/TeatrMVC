using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Theatr.BLL.DTO;
using Theatr.BLL.Infrastructure;
using Theatr.BLL.Interfaces;
using Theatr.DAL.Entities;
using Theatr.DAL.Repositories;

namespace Theatr.BLL.Service
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IdentityUnitOfWork UnitOfWork;

        public RegistrationService(IdentityUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        public void Register(UserDTO user)
        {
            if (user == null)
                throw new ValidationException("No info about user", "");
            var testUser = UnitOfWork.ClientProfiles.Find(u => u.Email.Equals(user.Email));
            if (testUser.Count() > 0)
                throw new ValidationException("Email is taken", "");
            var NewUser = new ClientProfile
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Password = user.Password
            };
            UnitOfWork.ClientProfiles.Create(NewUser);
            UnitOfWork.Save();
        }
       
        
    }
}
