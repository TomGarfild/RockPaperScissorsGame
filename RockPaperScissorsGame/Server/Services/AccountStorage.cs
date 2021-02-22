using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    public class AccountStorage : IAccountStorage
    {
        private readonly ConcurrentBag<Account> _storage  = new ConcurrentBag<Account>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public AccountStorage()
        {
            //_storage = 
        }

        public async Task<bool> AddAsync(Account account)
        {
            if (account == null) throw new NullReferenceException();
            if (_storage.Any(acc => acc.Id == account.Id)) return false;
            _storage.Add(account);
            
            return true;
        }

        public async Task<Account> FindAsync(string login, string password)
        {
            //TODO: read from JSON file
            return _storage.FirstOrDefault(acc => acc.Login == login && acc.Password == password);
        }
    }
}