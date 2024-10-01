using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ormb.runner;

public class Program
{
    static void Main(string[] args)
    {
        // create a list of types in this assembly that implement ORMBenchmarks<T>
        var types = typeof(Program).Assembly.GetTypes()
            .Where(t => t.IsClass
                    && !t.IsAbstract && t.BaseType?.IsGenericType == true
                    && t.BaseType.GetGenericTypeDefinition() == typeof(ORMBenchmarks<>))
            .ToList();

        var benchmark = MenuPrompt(
            "Select a benchmark class:",
            types.Select(t => t.Name).ToList());

        // create a list of ORMBenchmarks<T> methods that have the [Benchmark] attribute
        var methods = types[benchmark].GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(BenchmarkAttribute), false).Length > 0)
            .ToList();

        var method = MenuPrompt(
            "Select a benchmark method:",
            methods.Select(m => m.Name).ToList());
    }

    private static int MenuPrompt(string title, List<string> menuItems)
    {
        int selectedIndex = 0;
        string filter = "";

        while (true)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length));

            // Filter menu items
            List<string> filteredItems = menuItems
                .Where(item => item.ToLower().Contains(filter.ToLower()))
                .ToList();

            // Display menu items
            for (int i = 0; i < filteredItems.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {filteredItems[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {filteredItems[i]}");
                }
            }

            Console.WriteLine("\nFilter: " + filter);

            // Handle key presses
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(filteredItems.Count - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    Console.WriteLine($"Selected: {filteredItems[selectedIndex]}");
                    Console.ReadKey(true);
                    return selectedIndex;
                case ConsoleKey.Backspace:
                    if (filter.Length > 0)
                        filter = filter.Substring(0, filter.Length - 1);
                    break;
                default:
                    if (char.IsLetterOrDigit(keyInfo.KeyChar) || char.IsWhiteSpace(keyInfo.KeyChar))
                        filter += keyInfo.KeyChar;
                    break;
            }
        }
    }
}