using System.Collections.Generic;
using Theatr.BLL.DTO;

namespace Theatr.BLL.Interfaces
{
    public interface IAuthorizationService
    {
        UserDTO FindUser(string email);
        UserDTO FindUserById(int id);
        IEnumerable<TicketDTO> GetTicketsByUserId(int userId);
        UserDTO Login(string email, string password);
    }
}
