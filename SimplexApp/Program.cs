using SimplexLib;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

namespace SimplexApp;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        var fileOption = new Option<FileInfo?>(
            aliases: new String[] { "-lp", "--lpfile" },
            description: "Beschreibung");

        var rootCommand = new RootCommand(
            "Application to solve linear problems with the simplex algorithm");

        rootCommand.AddOption(fileOption);

        rootCommand.SetHandler((FileInfo file)
            => SimplexCLIHandle(file), fileOption);

        return await rootCommand.InvokeAsync(args);
    }

    static void SimplexCLIHandle(FileInfo fileInfo)
    {
        // Parse raw input into data structures
        var parsedProblem = new LPParser(fileInfo).Parse();

        LPSolver solver = new();

        // Arrange data structures to array table and header lists
        var table = solver.Arrange(
            parsedProblem.Item1, parsedProblem.Item2);

        // Solve problem
        List<(String, Double)> result = solver.Solve(table);

        foreach(var row in result)
            Console.WriteLine($"{row.Item1}: {row.Item2}");
    }
}