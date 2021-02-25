using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class GameMenu : Menu
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        public GameMenu(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
        }
        public override async Task Start()
        {
            bool changed = true;
            
            var roomMenu = new RoomMenu(_httpClient, _token);
            do
            {
                if (changed)
                {
                    PrintMenu("\t | Menu Rock Paper Scissors Game |",
                        new[]
                        {
                            "\t |     Public Room  - press 1    |",
                            "\t |     Private Room - press 2    |",
                            "\t |     Computer     - press 3    |",
                            "\t |     Statistic    - press 4    |",
                            "\t |     Exit         - press E    |"
                        });
                }

                changed = true;
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
                        await roomMenu.Start();
                        break;
                    case ConsoleKey.D4:
                        break;
                    case ConsoleKey.E:
                        return;
                    default:
                        changed = false;
                        break;
                }
            } while (true);
            
        }
    }
}