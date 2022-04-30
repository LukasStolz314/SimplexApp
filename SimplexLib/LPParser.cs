using System.Text.RegularExpressions;

namespace SimplexLib;
public class LPParser
{
    private String filePath { get; set; }
    public LPParser(FileInfo fileInfo)
    {
        filePath = fileInfo.FullName;
    }

    public (ObjFunction, List<Constraint>) Parse()
    {
        // Reads and parses every line into list
        List<String> allLines = File.ReadAllLines(filePath).ToList();

        // Get all relevant lines (clears comments)
        List<String> relLines = RemoveComments(allLines);

        // Retrieves the objective function
        ObjFunction objFunction = GetObjFunction(relLines.First());

        // Retrieves the constraints
        var constraintLines = relLines.Where(x => !x.StartsWith("min:")).ToList();
        List <Constraint> constraints = GetConstraints(constraintLines);

        return (objFunction, constraints);
    }

    private List<String> RemoveComments(List<String> lines)
    {
        List<String> result = new();

        // Remove every line that starts with //
        foreach(String line in lines)
        {
            if(!line.Trim().StartsWith("//"))
                result.Add(line);
        }

        return result;
    }

    private ObjFunction GetObjFunction(String expression)
    {
        // Evaluates problem type
        ProblemType problemType = expression.StartsWith("min:") 
            ? ProblemType.Minimize 
            : ProblemType.Maximize;

        // Filters out unnecesarry parts
        String filtered = expression
            .Replace("min:", "").Replace(";", "")
            .Trim();

        // Extracts value pairs from filtered expression
        List<(Int32 value, String varName)> valuesPairs = new();
        List<String> splittedPairs = filtered.Split("+").ToList();

        // Has to start at zero, since previous split function creates an empty field at index 0
        for(int i = 1; i < splittedPairs.Count; i++)
        {
            String[] split = splittedPairs[i].Split("*");
            valuesPairs.Add(new(Convert.ToInt32(split[0].Trim()), split[1].Trim()));
        }
            
        ObjFunction result = new(problemType, valuesPairs);

        return result;
    }

    private List<Constraint> GetConstraints(List<String> constraints)
    {
        List<Constraint> result = new();

        foreach (var constraint in constraints)
        {
            Int32 resultValue = GetConstraintResultFromString(constraint);

            List<(Int32, String)> valuePairs =
                GetConstraintValuePairsFromString(constraint);

            // Adds finised parsed Lines to the globals list
            Constraint lpLine = new(valuePairs, resultValue);
            result.Add(lpLine);
        }

        return result;
    }

    private Int32 GetConstraintResultFromString(String constraint)
    {
        // Identifies right side of the constraint and trims off whitespaces and semicolon
        String result = constraint.Split(">=")[1].Replace(";", "").Trim();
        return Convert.ToInt32(result);
    }

    private List<(Int32, String)> GetConstraintValuePairsFromString(String constraint)
    {
        List<(Int32 value, String varName)> result = new();

        // Clears line of complete right side 
        Regex regex = new Regex(@">=\d+;");
        String clearedLine = regex.Replace(constraint.Replace(" ", ""), "");

        // Goes through all valuePairs (e.g. 1*x0), splitted by the '+'
        List<String> splittedPairs = clearedLine.Split("+").ToList();

        // Has to start at zero, since previous split function creates an empty field at index 0
        for (int i = 1; i < splittedPairs.Count; i++)
        {
            // Splits valuePair and assigns it into a tuple
            var split = splittedPairs[i].Split('*');
            (Int32, String) tuple = new(Convert.ToInt32(split[0]), split[1]);

            result.Add(tuple);
        }

        return result;
    }
}
