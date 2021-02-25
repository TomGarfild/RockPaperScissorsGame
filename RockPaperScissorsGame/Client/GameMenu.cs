using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Server.Model;

namespace Client
{
    public class GameMenu : Menu
    {
        private readonly string _login;
        private readonly HttpClient _httpClient;
        public GameMenu(string login, HttpClient httpClient)
        {
            _login = login;
            _httpClient = httpClient;
        }
        public override async Task Start()
        {
            PrintMenu("\t | Menu Rock Paper Scissors Game |",
                new[]
                {
                    "\t |     Public Room  - press 1    |",
                    "\t |     Private Room - press 2    |",
                    "\t |     Computer     - press 3    |",
                    "\t |     Exit         - press E    |"
                });
            do
            {
                Console.Write("\r\t  Key: ");
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("1");
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("2");
                        break;
                    case ConsoleKey.D3:
                        var seriesUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "/series/NewTrainingSeries");
                        _httpClient.DefaultRequestHeaders.Add("x-login", _login);
                        _httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        var seriesJson = await (await _httpClient.GetAsync(seriesUri)).Content.ReadAsStringAsync();
                        var series = JsonSerializer.Deserialize<Series>(seriesJson);
                        _httpClient.DefaultRequestHeaders.Add("x-series", series?.Id);
                        /*
                         * case "Rock":
                           return OptionChoice.Rock;
                           case "Paper":
                           return OptionChoice.Paper;
                           case "Scissor":
                         */
                        _httpClient.DefaultRequestHeaders.Add("x-choice", "Rock");
                        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "/round/TrainingPlay");
                        var response = await _httpClient.GetAsync(uri);

                        Console.WriteLine($"Result: {response.Content.ReadAsStringAsync().Result}");
                        break;
                    case ConsoleKey.E:
                        return;
                }
            } while (true);
            
        }
    }
}