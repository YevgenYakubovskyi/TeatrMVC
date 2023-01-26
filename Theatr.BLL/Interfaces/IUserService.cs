using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Theatr.BLL.DTO;
using Theatr.BLL.Infrastructure;

namespace Theatr.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
    }
}
