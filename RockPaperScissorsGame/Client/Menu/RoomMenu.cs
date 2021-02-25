using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
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
            var exit = await SetHeaders();
            if (exit) return;
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

        private async Task<bool> SetHeaders()
        {
            var seriesUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + _seriesRoute);
            var seriesTask = _httpClient.GetAsync(seriesUri);

            if (_seriesRoute.Contains("Public"))
            {
                Console.WriteLine("\r\t  Trying to find your opponent. Press E to exit.");
                Console.Write("\r\t  Key: ");
                while (seriesTask.Status != TaskStatus.RanToCompletion)
                {
                    if (Console.KeyAvailable)
                    {
                        Console.Write("\r\t  Key: ");
                        var key = Console.ReadKey().Key;
                        if (key == ConsoleKey.E)
                        {
                            Console.WriteLine("\n\t  You exit from public session.");
                            await Task.Delay(1000);
                            return true;
                        }

                        Console.Write("\b");
                    }
                }
                Console.WriteLine("\n\t  Your opponent was found.");
            }

            var seriesJson = await (await seriesTask).Content.ReadAsStringAsync();
            var seriesId = JsonSerializer.Deserialize<Series>(seriesJson, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })?.Id;
            
            _httpClient.DefaultRequestHeaders.Add("x-series", seriesId);
            return false;
        }

        public void SetRoutes(string seriesRoute, string gameRoute)
        {
            _seriesRoute = seriesRoute;
            _gameRoute = gameRoute;
        }
    }
}