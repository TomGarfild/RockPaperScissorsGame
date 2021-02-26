using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Server.Models;

namespace Client.Menu
{
    public class RoomMenu : Menu
    {
        private readonly HttpClient _httpClient;
        private string _seriesRoute;
        private string _gameRoute;
        public RoomMenu(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public override async Task Start()
        {
            if (_seriesRoute.Contains("Private"))
            {
                var privateMenu = await PrivateMenuAsync();
                if (!privateMenu) return;
            }
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
                var result = response.Content.ReadAsAsync<string>().Result;
                if (result.Equals("Undefine"))
                {
                    await Task.Delay(3000);
                    Console.WriteLine("\nYour opponent didn't make a choice. You will be redirected to menu.");
                    return;
                }
                Console.WriteLine($"\nResult: {result}");
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
                await Task.Delay(1000);
            }

            var responce = await seriesTask;
            if ((int) responce.StatusCode == 404)
            {
                Console.WriteLine("\nRoom is full or not found");
                await Task.Delay(2000);
                return true;
            }
            var seriesJson = await (responce).Content.ReadAsStringAsync();
            var seriesId = JsonSerializer.Deserialize<Series>(seriesJson, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })?.Id;

            _httpClient.DefaultRequestHeaders.Remove("x-series");
            _httpClient.DefaultRequestHeaders.Add("x-series", seriesId);
            return false;
        }

        private async Task<bool> PrivateMenuAsync()
        {
            PrintMenu("|       Private Room Menu       |",
                new[]
                {

                    "|     Create Room  - press 1    |",
                    "|     Enter Room   - press 2    |",
                    "|     Exit         - press E    |"
                });
            do
            {
                Console.Write("\rKey: ");
                var key = Console.ReadKey().Key;
                var enter = false;
                switch (key)
                {
                    case ConsoleKey.D1:
                        Log.Information($"Get request {_httpClient.BaseAddress.AbsoluteUri + "/series/NewPrivateSeries"}");
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress.AbsoluteUri
                                                                  + "/series/NewPrivateSeries");
                        Log.Information($"Status code: {response.StatusCode}");
                        var privateJson = await response.Content.ReadAsStringAsync();
                        var code = JsonSerializer.Deserialize<PrivateSeries>(privateJson, new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        })?.Code;
                        Console.WriteLine($"\nCode: {code}. Now you and your friend can login this room.");
                        break;
                    case ConsoleKey.D2:
                        Console.Write("\nEnter room's code: ");
                        var entranceCode = Console.ReadLine();
                        _httpClient.DefaultRequestHeaders.Remove("x-code");
                        _httpClient.DefaultRequestHeaders.Add("x-code", entranceCode);
                        enter = true;
                        break;
                    case ConsoleKey.E:
                        return false;
                }

                if (enter) return true;
            } while (true);
        }

        public void SetRoutes(string seriesRoute, string gameRoute)
        {
            _seriesRoute = seriesRoute;
            _gameRoute = gameRoute;
        }
    }
}