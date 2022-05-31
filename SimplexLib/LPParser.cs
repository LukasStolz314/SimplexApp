using System.Text.RegularExpressions;

namespace SimplexLib;
public class LPParser
{
    private String filePath { get; set; }
    public LPParser(FileInfo fileInfo)
    {
        filePath = fileInfo.FullName;
    }

    public List<Dictionary<String, Double>> Parse()
    {
        // Reads and parses every line into list
        List<String> allLines = File.ReadAllLines(filePath).ToList();

        // Get all relevant lines (clears comments)
        List<String> relLines = RemoveComments(allLines);

        // Retrieves the objective function
        Dictionary<String, Double> firstRow = GetFirstRow(relLines.First());

        List<Dictionary<String, Double>> result = new () { firstRow };

        // Remove first row
        relLines.RemoveAt (0);

        // Gets constraints and adds them to the list
        var constraints = GetConstraints(relLines);
        result.AddRange (constraints);

        return result;
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

    private Dictionary<String, Double> GetFirstRow(String expression)
    {

        // Filters out unnecesarry parts
        String filtered = expression.Replace("min:", "").Replace(";", "").Trim();

        // Extracts value pairs from filtered expression
        Dictionary<String, Double> result = new();
        List<String> splittedPairs = filtered.Split("+").ToList();

        result.Add ("result", 0);

        // Has to start at zero, since previous split function creates an empty field at index 0
        for(Int32 i = 1; i < splittedPairs.Count; i++)
        {
            String[] split = splittedPairs[i].Split("*");
            result.Add(split[1].Trim(), Convert.ToDouble(split[0].Trim()));
        }      

        return result;
    }

    private List<Dictionary<String, Double>> GetConstraints(List<String> constraints)
    {
        List<Dictionary<String, Double>> result = new();

        foreach (var constraint in constraints)
        {
            Dictionary<String, Double> line = new (); 

            Double resultValue = GetConstraintResultFromString(constraint);
            line.Add ("result", resultValue);

            GetConstraintValuePairsFromString(constraint, ref line);

            result.Add(line);
        }

        return result;
    }

    private Double GetConstraintResultFromString(String constraint)
    {
        // Identifies right side of the constraint and trims off whitespaces and semicolon
        String result = constraint.Split(">=")[1].Replace(";", "").Trim();
        return Convert.ToDouble(result);
    }

    private void GetConstraintValuePairsFromString(
        String constraint, ref Dictionary<String, Double> dict)
    {
        // Clears line of complete right side 
        Regex regex = new Regex(@">=\d+;");
        String clearedLine = regex.Replace(constraint.Replace(" ", ""), "");

        // Goes through all valuePairs (e.g. 1*x0), splitted by the '+'
        List<String> splittedPairs = clearedLine.Split("+").ToList();

        // Has to start at zero, since previous split function creates an empty field at index 0
        for (Int32 i = 1; i < splittedPairs.Count; i++)
        {
            // Splits valuePair and assigns it into a tuple
            var split = splittedPairs[i].Split('*');
            var splitValue = Convert.ToDouble (split[0]);
            var splitName = split[1];

            dict.Add(splitName, splitValue);
        }
    }
}
