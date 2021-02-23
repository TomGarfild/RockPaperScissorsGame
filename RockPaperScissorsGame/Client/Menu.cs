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
        /*string reg = "http://localhost:5000/api/v1/account/login";
            var httpClient = new HttpClient() {BaseAddress = new Uri("http://localhost:5000")};
            var acc = new Account() {Login = "hello", Password = "1234heiii"};
            var cont = new StringContent(JsonSerializer.Serialize(acc), Encoding.UTF8, "application/json");
            var response  = await httpClient.PostAsync(new Uri(reg), cont);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.ReadKey();*/
        private readonly HttpClient _httpClient;

        public Menu(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Start()
        {
            Console.WriteLine("\n\t\t-+-------------------------------+-");
            Console.WriteLine("\t\t | Menu Rock Paper Scissors Game |");
            Console.WriteLine("\t\t-+-------------------------------+-");
            Console.WriteLine("\t\t |       Register - press R      |");
            Console.WriteLine("\t\t |       Login    - press L      |");
            Console.WriteLine("\t\t |       Exit     - press E      |");
            Console.WriteLine("\t\t-+-------------------------------+-");
            Console.Write("\n\t\t  Key: ");
            do
            {
                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.R:
                    case ConsoleKey.L:
                        var content = GetContent();
                        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri +
                                          (key == ConsoleKey.R ? "/register" : "/login"));
                        var response = await _httpClient.PostAsync(uri, content);
                        break;
                    case ConsoleKey.E:
                        return;
                }

                Console.Write('\b');
            } while (true);
        }

        private StringContent GetContent()
        {
            var acc = new Account() { Login = "hello", Password = "1234heiii" };
            var cont = new StringContent(JsonSerializer.Serialize(acc), Encoding.UTF8, "application/json");
            return null;
        }
    }
}