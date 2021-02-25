using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server.Models;

namespace Client
{
    public class RegistrationMenu : Menu
    {
        private readonly HttpClient _httpClient;
        public RegistrationMenu(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task Start()
        {
            PrintMenu("\t | Menu Rock Paper Scissors Game |", 
                new []
                {
                    "\t |       Register - press R      |",
                    "\t |       Login    - press L      |",
                    "\t |       Exit     - press E      |"
                });
            do
            {
                Console.Write("\r\t  Key: ");
                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.R:
                        var regContent = GetContent();
                        var regUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "/account/register");
                        var regResponse = await _httpClient.PostAsync(regUri, regContent);
                        if ((int) regResponse.StatusCode == 200)
                        {
                            Console.WriteLine("\t  Now you can login");
                        }
                        else
                        {
                            Console.WriteLine("\t  Login exists already");
                        }
                        break;
                    case ConsoleKey.L:
                        var loginContent = GetContent();
                        var loginUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "/account/login");
                        var loginResponse = await _httpClient.PostAsync(loginUri, loginContent);
                        if ((int) loginResponse.StatusCode == 200)
                        {
                            Console.WriteLine("\t  Your LogIn was successful.");
                            await Task.Delay(1000);
                            var menu = new GameMenu("tom", _httpClient);
                            await menu.Start();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("\t  Such user doesn't exist");
                        }
                        break;
                    case ConsoleKey.E:
                        return;
                }

                Console.Write('\b');
            } while (true);
        }
        protected static StringContent GetContent()
        {
            Console.WriteLine();
            var login = GetField("login", 3, 20);
            var password = GetField("password", 6, 64);

            var account = new Account()
            {
                Login = login,
                Password = password
            };
            var json = JsonSerializer.Serialize(account);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }
    }
}