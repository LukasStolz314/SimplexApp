namespace SimplexLib;
public class Simplex
{
    public Dictionary<String, Double> Solve(FileInfo input)
    {
        // Parse raw input into data structures
        var lines = File.ReadAllLines(input.FullName).ToList();
        var parsedProblem = new LPParser().Parse(lines);

        LPPrint.Print("Initial");
        LPPrint.PrintTable(parsedProblem);

        var list = LPTransposer.Transpose (parsedProblem);

        LPPrint.Print("Transposed");
        LPPrint.PrintTable(list);

        var result = new LPSolver(list).Solve();

        LPPrint.PrintResult(result);

        return result;
    }

    public Dictionary<String, Double> Solve(String input)
    {
        // Parse raw input into data structures
        List<String> lines = input.Replace ("\r", "").Split ("\n").ToList ();
        var parsedProblem = new LPParser().Parse(lines);

        LPPrint.Print("Initial");
        LPPrint.PrintTable(parsedProblem);

        var list = LPTransposer.Transpose (parsedProblem);

        LPPrint.Print("Transposed");
        LPPrint.PrintTable(list);

        var result = new LPSolver(list).Solve();

        LPPrint.Print($"Result: {result}");

        return result;
    }
}
