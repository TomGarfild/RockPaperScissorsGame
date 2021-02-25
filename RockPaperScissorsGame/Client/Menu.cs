using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public abstract class Menu
    {
        protected readonly string Token;
        protected readonly HttpClient Client;

        protected Menu()
        {

        }

        protected Menu(string token)
        {
            Token = token;
        }

        protected Menu(HttpClient client)
        {
            Client = client;
        }

        public abstract Task Start();
        protected static void PrintMenu(string header, string[] fields)
        {
            Console.Clear();
            Console.WriteLine("\n\t-+-------------------------------+-");
            Console.WriteLine(header);
            Console.WriteLine("\t-+-------------------------------+-");
            foreach (var field in fields)
            {
                Console.WriteLine(field);
            }
            Console.WriteLine("\t-+-------------------------------+-");
        }

        protected static string GetField(string name, int min, int max)
        {
            Console.Write($"\t  Enter {name}: ");
            var field = Console.ReadLine()?.Trim();
            while (field != null && (field.Length < min || field.Length > max))
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write("\r" + new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"\t  Enter {name}(min length = {min}, max length = {max}): ");
                field = Console.ReadLine()?.Trim();
            }
            return field;
        }
    }
}