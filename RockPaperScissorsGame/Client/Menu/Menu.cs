using System;
using System.Threading.Tasks;

namespace Client.Menu
{
    public abstract class Menu
    {
        public abstract Task Start();
        protected static void PrintMenu(string header, string[] fields)
        {
            Console.Clear();
            Console.WriteLine("\n+-------------------------------+");
            Console.WriteLine(header);
            Console.WriteLine("+-------------------------------+");
            foreach (var field in fields)
            {
                Console.WriteLine(field);
            }
            Console.WriteLine("+-------------------------------+");
        }

        protected static string GetField(string name, int min, int max)
        {
            var cursor = Console.CursorTop;
            Console.Write($"Enter {name}: ");
            var field = Console.ReadLine()?.Trim();
            
            while (field != null && (field.Length < min || field.Length > max))
            {
                Console.SetCursorPosition(0,  cursor);
                Console.Write("\r" + new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, cursor);
                Console.Write($"\rEnter {name}(min length = {min}, max length = {max}): ");
                field = Console.ReadLine()?.Trim();
            }
            return field;
        }
    }
}