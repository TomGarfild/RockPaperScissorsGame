using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Server;
using Server.Model;

namespace Client
{
    public class RoomMenu : Menu
    {
        private readonly HttpClient _httpClient;
        private readonly string _login;

        public RoomMenu(string login, HttpClient httpClient)
        {
            _login = login;
            _httpClient = httpClient;
        }
        public override async Task Start()
        {
            PrintMenu("\t |         Room Menu         |",
                new[]
                {
                    "\t |    Rock      - press R    |",
                    "\t |    Paper     - press P    |",
                    "\t |    Scissors  - press S    |",
                    "\t |    Exit Room - press E    |"
                });
            do
            {
                Console.Write("\r\t  Key: ");
                var key = Console.ReadKey().Key;
                var answer = string.Empty;
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
                }

                var seriesUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "/series/NewTrainingSeries");
                _httpClient.DefaultRequestHeaders.Add("x-login", _login);
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var seriesJson = await(await _httpClient.GetAsync(seriesUri)).Content.ReadAsStringAsync();
                var series = JsonSerializer.Deserialize<Series>(seriesJson);
                _httpClient.DefaultRequestHeaders.Add("x-series", series?.Id);
                _httpClient.DefaultRequestHeaders.Add("x-choice", answer);
                var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "/round/TrainingPlay");
                var response = await _httpClient.GetAsync(uri);

                Console.WriteLine($"Result: {response.Content.ReadAsStringAsync().Result}");
            } while (true);

        }
    }
}