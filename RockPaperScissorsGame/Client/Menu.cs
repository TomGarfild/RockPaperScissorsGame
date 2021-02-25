using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server.Models;

namespace Client
{
    public class Menu
    {
        private readonly HttpClient _httpClient;

        public Menu(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Start()
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
                        var regUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "/register");
                        var regResponse = await _httpClient.PostAsync(regUri, regContent);
                        if ((int) regResponse.StatusCode == 200)
                        {
                            Console.WriteLine("\t Now you can login");
                        }
                        else
                        {
                            Console.WriteLine("\t  Login exists already");
                        }
                        break;
                    case ConsoleKey.L:
                        var loginContent = GetContent();
                        var loginUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "/register");
                        var loginResponse = await _httpClient.PostAsync(loginUri, loginContent);
                        if ((int)loginResponse.StatusCode == 200)
                        {
                            PrintMenu("\t | Menu Rock Paper Scissors Game |",
                                new[]
                                {
                                    "\t |     Public Room  - press 1    |",
                                    "\t |     Private Room - press 2    |",
                                    "\t |     Computer     - press 3    |",
                                    "\t |     Exit         - press E    |"
                                });
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

        private static StringContent GetContent()
        {
            Console.WriteLine();
            var login = GetField("login", 3, 20);
            var password = GetField("password", 6, 64);
            
            var account = new Account()
            {
                Login = login, Password = password
            };
            var json = JsonSerializer.Serialize(account);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        private static void PrintMenu(string header, string[] fields)
        {
            Console.Clear();
            Console.WriteLine("\n\t-+-------------------------------+-");
            Console.WriteLine(header);
            Console.WriteLine("\t-+-------------------------------+-");
            foreach (var field in fields)
            {
                Console.WriteLine(field);
            }
            Console.WriteLine("\t-+-------------------------------+-");
        }

        private static string GetField(string name, int min, int max)
        {
            Console.Write($"\t  Enter {name}: ");
            var field = Console.ReadLine()?.Trim();
            while (field != null && (field.Length < min || field.Length > max))
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write("\r" + new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write($"\t  Enter {name}(min length = {min}, max length = {max}): ");
                field = Console.ReadLine()?.Trim();
            }
            return field;
        }
    }
}