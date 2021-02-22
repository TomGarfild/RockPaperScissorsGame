using System.Threading.Tasks;

namespace Server.Services
{
    public interface IAccountStorage
    {
        public Task<bool> AddAsync(Account account);

        public Account Find(string login, string password);
    }
}