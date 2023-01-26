using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Theatr.BLL.Interfaces;
using Theatr.BLL.Service;
using Theatr.DAL.Interfaces;
using Theatr.DAL.Repositories;

namespace Theatr.BLL.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernal;
        private string connectionString;
        public NinjectDependencyResolver(string connection)
        {
            connectionString = connection;
            kernal = new StandardKernel();
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernal.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernal.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernal.Bind<IAuthorizationService>().To<AuthorizationService>();
            kernal.Bind<IManagePerfomanceService>().To<ManagePerfomanceService>();
            kernal.Bind<IRegistrationService>().To<RegistrationService>();
            kernal.Bind<IUserService>().To<UserService>();
            kernal.Bind<IUnitOfWork>().To<IdentityUnitOfWork>().WithConstructorArgument(connectionString);
        }

    }
}
