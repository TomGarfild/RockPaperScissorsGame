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
            var exit = await SetHeaders();
            if (exit) return;

            PrintMenu("|           Room Menu           |",
                new[]
                {
                    "|     Rock       -  press R     |",
                    "|     Paper      -  press P     |",
                    "|     Scissors   -  press S     |",
                    "|     Exit Room  -  press E     |"
                });
            
            do
            {
                Console.Write("\rKey: ");
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

                Console.WriteLine($"\nResult: {response.Content.ReadAsStringAsync().Result}");
            } while (true);
        }

        private async Task<bool> SetHeaders()
        {
            var seriesUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + _seriesRoute);
            var seriesTask = _httpClient.GetAsync(seriesUri);

            if (_seriesRoute.Contains("Public"))
            {
                Console.WriteLine("\rTrying to find your opponent. Press E to exit.");
                Console.Write("\rKey: ");
                while (seriesTask.Status != TaskStatus.RanToCompletion)
                {
                    if (Console.KeyAvailable)
                    {
                        Console.Write("\rKey: ");
                        var key = Console.ReadKey().Key;
                        if (key == ConsoleKey.E)
                        {
                            Console.WriteLine("\nYou exit from public session.");
                            await Task.Delay(1000);
                            return true;
                        }

                        Console.Write("\b");
                    }
                }
                Console.WriteLine("\nYour opponent was found.");
            }
            else if (_seriesRoute.Contains("Private"))
            {
                var privateSeriesJson = await (await seriesTask).Content.ReadAsStringAsync();
                var privateSeries = JsonSerializer.Deserialize<PrivateSeries>(privateSeriesJson, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                _httpClient.DefaultRequestHeaders.Remove("x-series");
                _httpClient.DefaultRequestHeaders.Add("x-series", privateSeries.Id);

                _httpClient.DefaultRequestHeaders.Remove("x-code");
                _httpClient.DefaultRequestHeaders.Add("x-code", privateSeries.Code);
                return false;
            }

            var seriesJson = await (await seriesTask).Content.ReadAsStringAsync();
            var seriesId = JsonSerializer.Deserialize<Series>(seriesJson, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })?.Id;

            _httpClient.DefaultRequestHeaders.Remove("x-series");
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