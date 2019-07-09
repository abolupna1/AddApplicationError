using System.Collections.Generic;
using System.Threading.Tasks;
using ZwajApp.API.Models;

namespace ZwajApp.API.Data
{
    public interface IZwagRepository
    {
         void Add <T>(T entity) where T:class;
         void Delete <T>(T entity) where T:class;

        Task<bool> SavaAll();

        Task<IEnumerable<User>> GetUsers();

        Task<User> GetUser(int id);
        
    }
}