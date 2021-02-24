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
            Console.Clear();
            Console.WriteLine("\n\t-+-------------------------------+-");
            Console.WriteLine("\t | Menu Rock Paper Scissors Game |");
            Console.WriteLine("\t-+-------------------------------+-");
            Console.WriteLine("\t |       Register - press R      |");
            Console.WriteLine("\t |       Login    - press L      |");
            Console.WriteLine("\t |       Exit     - press E      |");
            Console.WriteLine("\t-+-------------------------------+-");
            
            do
            {
                Console.Write("\r\t  Key: ");
                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.R:
                    case ConsoleKey.L:
                        var content = GetContent();
                        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri +
                                          (key == ConsoleKey.R ? "/register" : "/login"));
                        var response = await _httpClient.PostAsync(uri, content);
                        switch ((int)response.StatusCode)
                        {
                            case 200:
                                Console.WriteLine("\t  OK");
                                break;
                            case 409:
                                Console.WriteLine("\t  Login exists already");
                                break;
                            case 404:
                                Console.WriteLine("\t  Such user wasn't found");
                                break;
                            default:
                                Console.WriteLine("\t  Something went wrong");
                                break;
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