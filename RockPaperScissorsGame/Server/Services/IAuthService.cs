using System.Threading.Tasks;

namespace Server.Services
{
    public interface IAuthService
    {
        public Task<bool> Register(string login, string password);
        public Task<string> Login(string login, string password);
        public bool IsAuthorized(string token);
    }
}