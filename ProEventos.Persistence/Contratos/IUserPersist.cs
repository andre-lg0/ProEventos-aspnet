using System.Threading.Tasks;
using System.Collections.Generic;
using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Contratos
{
    public interface IUserPersist
    {
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<User> GetUserByIdAsync(int id);
        public Task<User> GetUserByUsernameAsync(string username);

        
    }
}