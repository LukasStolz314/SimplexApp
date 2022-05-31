namespace SimplexLib;
public class Simplex
{
    public Dictionary<String, Double> Solve(FileInfo input)
    {
        LPPrint.IsActive = false;

        // Parse raw input into data structures
        var lines = File.ReadAllLines(input.FullName).ToList();
        var parsedProblem = new LPParser().Parse(lines);

        Console.WriteLine ("Initial \r\n");
        LPPrint.Print (parsedProblem);

        var list = LPTransposer.Transpose (parsedProblem);

        Console.WriteLine ("Transposed \r\n");
        LPPrint.Print(list);

        var result = new LPSolver(list).Solve();

        Console.WriteLine ($"Result: {result}");

        return result;
    }

    public Dictionary<String, Double> Solve(String input)
    {
        LPPrint.IsActive = true;

        // Parse raw input into data structures
        List<String> lines = input.Replace ("\r", "").Split ("\n").ToList ();
        var parsedProblem = new LPParser().Parse(lines);

        Console.WriteLine ("Initial \r\n");
        LPPrint.Print (parsedProblem);

        var list = LPTransposer.Transpose (parsedProblem);

        Console.WriteLine ("Transposed \r\n");
        LPPrint.Print(list);

        var result = new LPSolver(list).Solve();

        Console.WriteLine ($"Result: {result}");

        return result;
    }
}
