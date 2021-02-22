using System.Threading.Tasks;

namespace Server.Services
{
    public interface IAuthService
    {
        public Task<bool> Register(string login, string password);
        public string Login(string login, string password);
    }
}