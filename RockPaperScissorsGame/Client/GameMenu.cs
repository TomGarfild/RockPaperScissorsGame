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
            var roomMenu = new RoomMenu(_login, _httpClient);
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
                        await roomMenu.Start();
                        break;
                    case ConsoleKey.E:
                        return;
                }
            } while (true);
            
        }
    }
}