using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Services
{
    public class AuthService : IAuthService

    {
        private readonly Dictionary<string, string> _tokens = new Dictionary<string, string>();
        private readonly IAccountStorage _accounts;

        public AuthService(IAccountStorage accounts)
        {
            _accounts = accounts;
        }

        public async Task<bool> Register(string login, string password)
        {
            return await _accounts.AddAsync(new Account()
            {
                Id = Guid.NewGuid().ToString(),
                Login = login,
                Password = password
            });
        }

        public async Task<string> Login(string login, string password)
        {
            var account = await _accounts.FindAsync(login, password);
            if (account == null) return null;

            var token = Guid.NewGuid().ToString();
            _tokens.Add(token, account.Id);

            return token;
        }

        public bool IsAuthorized(string token)
        {
            if (token == null || !_tokens.ContainsKey(token)) return false;
            return true;
        }
    }
}