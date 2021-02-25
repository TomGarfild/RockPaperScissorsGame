using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Server.Model;

namespace Client.Menu
{
    public class RoomMenu : Menu
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private string _seriesRoute;
        private string _gameRoute;
        public RoomMenu(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
        }
        public override async Task Start()
        {
            PrintMenu("\t |           Room Menu           |",
                new[]
                {
                    "\t |     Rock       -  press R     |",
                    "\t |     Paper      -  press P     |",
                    "\t |     Scissors   -  press S     |",
                    "\t |     Exit Room  -  press E     |"
                });
            await SetHeaders();
            do
            {
                Console.Write("\r\t  Key: ");
                var key = Console.ReadKey().Key;
                string answer;
                switch (key)
                {
                    case ConsoleKey.R:
                        answer = "Rock";
                        break;
                    case ConsoleKey.P:
                        answer = "Paper";
                        break;
                    case ConsoleKey.S:
                        answer = "Scissors";
                        break;
                    case ConsoleKey.E:
                        return;
                    default:
                        continue;
                }

                _httpClient.DefaultRequestHeaders.Remove("x-choice");
                _httpClient.DefaultRequestHeaders.Add("x-choice", answer);
                var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + _gameRoute);
                var response = await _httpClient.GetAsync(uri);

                Console.WriteLine($"\n\t  Result: {response.Content.ReadAsStringAsync().Result}");
            } while (true);
        }

        private async Task SetHeaders()
        {
            var seriesUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + _seriesRoute);
            var seriesTask =  _httpClient.GetAsync(seriesUri);

            if (_seriesRoute.Contains("Public"))
            {
                while (seriesTask.Status != TaskStatus.RanToCompletion)
                {
                    Console.Write("\r\t  Trying to find your opponent.");
                    await Task.Delay(500);
                    Console.Write(".");
                    await Task.Delay(500);
                    Console.Write(".");
                }
            }

            var seriesJson = await (await seriesTask).Content.ReadAsStringAsync();
            var seriesId = JsonSerializer.Deserialize<Series>(seriesJson, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })?.Id;
            
            _httpClient.DefaultRequestHeaders.Add("x-series", seriesId);
        }

        public void SetRoutes(string seriesRoute, string gameRoute)
        {
            _seriesRoute = seriesRoute;
            _gameRoute = gameRoute;
        }
    }
}