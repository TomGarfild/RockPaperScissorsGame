using System.Threading.Tasks;
using Server.Models;

namespace Server.Services
{
    public interface IAccountStorage
    {
        public Task<bool> AddAsync(Account account);

        public Task<Account> FindAsync(string login, string password);
        public string FindById(string id);
    }
}
