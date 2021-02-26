using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Services
{
    public class AuthService : IAuthService

    {
        private readonly Dictionary<string, string> _tokens = new Dictionary<string, string>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IAccountStorage _accounts;

        public AuthService(IAccountStorage accounts)
        {
            _accounts = accounts;
        }

        public async Task<bool> Register(string login, string password)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _accounts.AddAsync(new Account()
                {
                    Id = Guid.NewGuid().ToString(),
                    Login = login,
                    Password = password
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<string> Login(string login, string password)
        {
            var account = await _accounts.FindAsync(login, password);
            if (account == null) return null;
            if (_tokens.ContainsValue((await _accounts.FindAsync(login, password)).Id)) throw new MultiDeviceException("You cannot login in several devices at the same time.");

            var token = Guid.NewGuid().ToString();
            _tokens.Add(token, account.Id);

            return token;
        }

        public async Task<bool> Logout(string token)
        {
            await _semaphore.WaitAsync();
            try
            {
                return _tokens.Remove(token);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public bool IsAuthorized(string token)
        {
            _semaphore.Wait();
            try
            {
                if (token == null || !_tokens.ContainsKey(token)) return false;
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public string GetLogin(string token)
        {

            _semaphore.Wait();
            try
            {
                if (token == null || !_tokens.ContainsKey(token)) return null;
            return _accounts.FindById(_tokens[token]);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
