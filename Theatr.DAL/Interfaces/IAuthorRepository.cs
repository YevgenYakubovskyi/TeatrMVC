using System.Collections.Generic;
using Theatr.DAL.Entities;

namespace Theatr.DAL.Interfaces
{
    public interface IAuthorRepository
    {
        Author GetByName(string name);
    }
}