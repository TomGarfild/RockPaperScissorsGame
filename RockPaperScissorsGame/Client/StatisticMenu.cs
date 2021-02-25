using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class StatisticMenu : Menu
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        public StatisticMenu(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
        }
        public override async Task Start()
        {
            do
            {
                PrintMenu("\t |        Statistics Menu        |",
                    new[]
                    {
                        
                        "\t |   Local Statistic  - press 1  |",
                        "\t |   Global Statistic - press 2  |",
                        "\t |   Exit             - press E  |"
                    });
                Console.Write("\r\t  Key: ");
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1:

                        break;
                    case ConsoleKey.D2:

                        break;
                    case ConsoleKey.E:
                        return;
                }
            } while (true);
        }
    }
}