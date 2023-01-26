using System.Collections.Generic;
using Theatr.BLL.DTO;

namespace Theatr.BLL.Interfaces
{
    public interface IAuthorizationService
    {
        UserDTO FindUser(string email);
        UserDTO FindUserById(string id);
        IEnumerable<TicketDTO> GetTicketsByUserId(string userId);
        UserDTO Login(string email, string password);
    }
}
