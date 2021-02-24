using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Services
{
    public class AccountStorage : IAccountStorage
    {
        private List<Account> _storage  = new List<Account>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly JsonWorker<List<Account>> _jsonWorker;

        public AccountStorage(JsonWorker<List<Account>> jsonWorker)
        {
            _jsonWorker = jsonWorker;
        }

        public async Task<bool> AddAsync(Account account)
        {
            if (account == null) throw new NullReferenceException();
            if (_storage.Count == 0)
            {
                _storage = await _jsonWorker.ReadAllAsync();
            }
            if (_storage.Any(acc => acc.Login == account.Login || acc.Id == account.Id)) return false;

            _storage.Add(account);
            await _jsonWorker.WriteAllAsync(_storage);

            return true;
        }

        public async Task<Account> FindAsync(string login, string password)
        {
            if (_storage.Count == 0)
            {
                _storage = await _jsonWorker.ReadAllAsync();
            }

            return _storage.FirstOrDefault(acc => acc.Login == login && acc.Password == password);
        }
    }
}