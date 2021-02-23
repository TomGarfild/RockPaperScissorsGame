using System.Threading.Tasks;

namespace Server.Services
{
    public interface IAccountStorage
    {
        public Task<bool> AddAsync(Account account);

        public Task<Account> FindAsync(string login, string password);
    }
}