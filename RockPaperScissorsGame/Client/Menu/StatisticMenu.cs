using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Menu
{
    public class StatisticMenu : Menu
    {
        private readonly HttpClient _httpClient;
        public StatisticMenu(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress.AbsoluteUri + "/statistic/LocalStatistic");
                        break;
                    case ConsoleKey.D2:
                        var responseGlobal = await _httpClient.GetAsync(_httpClient.BaseAddress.AbsoluteUri + "/statistic/GlobalStatistic");
                        break;
                    case ConsoleKey.E:
                        return;
                }
            } while (true);
        }
    }
}