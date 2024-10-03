using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ormb.runner;

public class Program
{
    static void Main(string[] args)
    {
        // create a list of types in this assembly that implement ORMBenchmarks<T>
        var types = typeof(Program)
            .Assembly.GetTypes()
            .Where(t =>
                t.IsClass
                && !t.IsAbstract
                && t.BaseType?.IsGenericType == true
                && t.BaseType.GetGenericTypeDefinition() == typeof(ORMBenchmarks<>)
            )
            .ToList();

        var benchmark = MenuPrompt(
            "",
            "Select a benchmark class:",
            types.Select(t => t.Name).ToList()
        );

        // create a list of ORMBenchmarks<T> methods that have the [Benchmark] attribute
        var methods = types[benchmark]
            .GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(BenchmarkAttribute), false).Length > 0)
            .ToList();

        var method = MenuPrompt(
            types[benchmark].Name,
            "Select a benchmark method:",
            methods.Select(m => m.Name).ToList()
        );

        var type = types[benchmark];
        var methodInfo = methods[method];
        var reportTitle = $"{type.Name}.{methodInfo.Name}";

        Console.Clear();
        WriteTitle(reportTitle);
        Console.WriteLine();

        BenchmarkRunner.Run(type, [methodInfo]);
        Console.WriteLine();

        WriteTitle(reportTitle);
        OrganizeReportArtifacts(type.Name, reportTitle);
    }

    private static void OrganizeReportArtifacts(
        string type,
        string reportTitle,
        [CallerFilePath] string callerFilePath = ""
    )
    {
        WriteMessage("Organizing report artifacts...");
        var programFile = new FileInfo(callerFilePath);
        var artifactsDir = programFile
            .Directory!.Parent!.GetDirectories("BenchmarkDotNet.Artifact")
            .Single();
        var resultsDir = artifactsDir.GetDirectories("results").Single();

        var logFile = artifactsDir
            .GetFiles(
                "*.log", ///////////////////////////
                SearchOption.TopDirectoryOnly
            )
            .Single();

        var resultsName = logFile.Name.Replace(type, reportTitle);
        resultsName = resultsName.Replace("ormb.runner.", "");
        logFile.MoveTo(Path.Combine(resultsDir.FullName, resultsName));

        resultsName = resultsName.Replace(".log", "");
        resultsDir.MoveTo(Path.Combine(artifactsDir.FullName, resultsName));
        resultsDir.Refresh();

        resultsDir.GetFiles("*").ToList().ForEach(f =>
        {
            if (f.Extension == ".log")
            {
                return;
            }
            var split = Regex.Split(f.Name, "-report");
            f.MoveTo(Path.Combine(resultsDir.FullName, $@"{resultsName}-report{split[1]}"));
        });

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Done!");
        Console.ResetColor();
    }

    private static void WriteTitle(string title)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(title);
        Console.ResetColor();
    }

    private static void WriteMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static int MenuPrompt(string title, string prompt, List<string> menuItems)
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            if (!string.IsNullOrEmpty(title))
            {
                WriteTitle(title);
                Console.WriteLine();
            }
            WriteMessage(prompt);
            Console.WriteLine(new string('-', prompt.Length));

            // Display menu items
            for (var i = 0; i < menuItems.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {menuItems[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {menuItems[i]}");
                }
            }
            // Handle key presses
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(menuItems.Count - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    return selectedIndex;
                default:
                    break;
            }
        }
    }
}
