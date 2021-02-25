using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Menu;

namespace Client
{
    public class Program
    {
        public static async Task Main()
        {
            var httpClient = await GetHttpClient("settings.json");
            var menu = new RegistrationMenu(httpClient);
            await menu.Start();
        }
        private static async Task<HttpClient> GetHttpClient(string path)
        {
            var stream = File.OpenRead(path);
            var settings = await JsonSerializer.DeserializeAsync<Settings>(stream);
            return new HttpClient()
            {
                BaseAddress = new Uri(settings.BaseAddress)
            };
        }
    } 
}
