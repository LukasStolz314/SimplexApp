using SimplexLib;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Diagnostics;

namespace SimplexApp;

internal class Program
{
    static async Task<Int32> Main(String[] args)
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
        LPPrint.IsActive = false;

        // Parse raw input into data structures
        var parsedProblem = new LPParser(fileInfo).Parse();        

        Console.WriteLine ("Initial \r\n");
        LPPrint.Print (parsedProblem);

        var list = LPTransposer.Transpose (parsedProblem);

        Console.WriteLine ("Transposed \r\n");
        LPPrint.Print(list);

        var result = new LPSolver(list).Solve();

        Console.WriteLine ($"Result: {result}");
    }
}