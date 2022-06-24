using SimplexLib;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Diagnostics;

namespace SimplexApp;

internal class Program
{
    static async Task Main(String[] args)
    {
        var fileOption = new Option<FileInfo?>(
            aliases: new String[] { "-lp", "--lpfile" },
            description: "Beschreibung");

        var rootCommand = new RootCommand(
            "Application to solve linear problems with the simplex algorithm");

        rootCommand.AddOption(fileOption);

        rootCommand.SetHandler((FileInfo file)
            => new Simplex().Solve(file), fileOption);

        await rootCommand.InvokeAsync(args);
    }
}