using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Menu
{
    public class GameMenu : Menu
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly MediaTypeWithQualityHeaderValue _mediaType;
        public GameMenu(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
            _mediaType = new MediaTypeWithQualityHeaderValue("application/json");
        }
        public override async Task Start()
        {
            bool changed = true;
            
            var roomMenu = new RoomMenu(_httpClient, _token);

            if (!_httpClient.DefaultRequestHeaders.Contains("x-token"))
            {
                _httpClient.DefaultRequestHeaders.Add("x-token", _token);
            }
            if (!_httpClient.DefaultRequestHeaders.Accept.Contains(_mediaType))
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(_mediaType);
            }
            do
            {
                if (changed)
                {
                    PrintMenu("| Menu Rock Paper Scissors Game |",
                        new[]
                        {
                            "|     Public Room  - press 1    |",
                            "|     Private Room - press 2    |",
                            "|     Computer     - press 3    |",
                            "|     Statistic    - press 4    |",
                            "|     Exit         - press E    |"
                        });
                }

                changed = true;
                Console.Write("\rKey: ");
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        roomMenu.SetRoutes("/series/NewPublicSeries", "/round/Play");
                        await roomMenu.Start();
                        break;
                    case ConsoleKey.D2:
                        roomMenu.SetRoutes("/series/SearchPrivateSeries", "/round/Play");
                        await roomMenu.Start();
                        break;
                    case ConsoleKey.D3:
                        roomMenu.SetRoutes("/series/NewTrainingSeries", "/round/TrainingPlay");
                        await roomMenu.Start();
                        break;
                    case ConsoleKey.D4:
                        var statistic = new StatisticMenu(_httpClient);
                        await statistic.Start();
                        break;
                    case ConsoleKey.E:
                        await _httpClient.DeleteAsync(new Uri(_httpClient.BaseAddress.AbsoluteUri + "/account/logout/" +
                                                              _token));
                        return;
                    default:
                        changed = false;
                        break;
                }
            } while (true);
            
        }
    }
}