using System.Diagnostics;

namespace SimplexLib;
public class Simplex
{
    public void Solve(FileInfo input)
    {
        Stopwatch sw = new ();
        sw.Start ();

        // Parse raw input into data structures
        var lines = File.ReadAllLines(input.FullName).ToList();
        var parsedProblem = new LPParser().Parse(lines);

        LPPrint.Print("Initial");
        LPPrint.PrintInitialTable(parsedProblem);

        var list = LPTransposer.Transpose (parsedProblem);

        LPPrint.Print("Transposed");
        LPPrint.PrintTable(list);

        var result = new LPSolver(list).Solve();

        sw.Stop ();

        LPPrint.PrintResult(result, sw.Elapsed);
    }
}
