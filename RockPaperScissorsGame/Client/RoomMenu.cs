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

                Client.DefaultRequestHeaders.Remove("x-choice");
                Client.DefaultRequestHeaders.Add("x-choice", answer);
                var uri = new Uri(Client.BaseAddress.AbsoluteUri + "/round/TrainingPlay");
                var response = await Client.GetAsync(uri);

                Console.WriteLine($"\n\t  Result: {response.Content.ReadAsStringAsync().Result}");
            } while (true);
        }

        private async Task SetHeaders()
        {
            var seriesUri = new Uri(Client.BaseAddress.AbsoluteUri + "/series/NewTrainingSeries");
            Client.DefaultRequestHeaders.Add("x-token", Token);
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var seriesJson = await(await Client.GetAsync(seriesUri)).Content.ReadAsStringAsync();
            var seriesId = JsonSerializer.Deserialize<Series>(seriesJson, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })?.Id;
            Client.DefaultRequestHeaders.Add("x-series", seriesId);
        }
    }
}