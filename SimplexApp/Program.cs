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
        var lines = new LPParser(fileInfo).Parse();
    }
}