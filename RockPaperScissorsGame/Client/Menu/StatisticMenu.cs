using System;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;

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
            PrintMenu("|        Statistics Menu        |",
                new[]
                {

                    "|   Local Statistic  - press 1  |",
                    "|   Global Statistic - press 2  |",
                    "|   Exit             - press E  |"
                });
            do
            {
                Console.Write("\rKey: ");
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        Log.Information($"Get request: {_httpClient.BaseAddress.AbsoluteUri + "/statistic/LocalStatistic"}");
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress.AbsoluteUri + "/statistic/LocalStatistic");
                        Log.Information($"Status cod: {response.StatusCode}");
                        var stat = await response.Content.ReadAsAsync<string>();
                        
                        Console.WriteLine("\n"+stat);
                        break;
                    case ConsoleKey.D2:
                        Log.Information($"Get request: {_httpClient.BaseAddress.AbsoluteUri + "/statistic/GlobalStatistic"}");
                        var responseGlobal = await _httpClient.GetAsync(_httpClient.BaseAddress.AbsoluteUri + "/statistic/GlobalStatistic");
                        Log.Information($"Status cod: {responseGlobal.StatusCode}");

                        var statGlobal = await responseGlobal.Content.ReadAsAsync<string>();
                        Console.WriteLine("\n"+statGlobal);
                        break;
                    case ConsoleKey.E:
                        return;
                }
            } while (true);
        }
    }
}