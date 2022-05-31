namespace SimplexLib;

public static class LPTransposer
{
    public static List<Dictionary<String, Double>> Transpose(
        List<Dictionary<String, Double>> list)
    {
        List<String> variables = list.First().Keys.ToList();

        var transposedList = TransposeToList (variables, list);

        var result = ToDict (variables, transposedList);

        result = AddSlackVariables(result);        

        return result;
    }

    private static List<List<Double>> TransposeToList(
        List<String> variables, List<Dictionary<String, Double>> list)
    {
        List<List<Double>> result = new ();

        // Initialize lists
        for (Int32 i = 0; i < variables.Count; i++)
            result.Add (new ());     
            
        // Transpose every entry based on variable name
        foreach(var item in list)
        {
            for(Int32 i = 0; i < variables.Count; i++)
            {
                var pair = item.First (x => x.Key.Equals (variables[i]));
                result[i].Add(pair.Value);
            }
        }

        return result;
    }

    private static List<Dictionary<String, Double>> ToDict(
        List<String> variables, List<List<Double>> transposed)
    {
        List<Dictionary<String, Double>> result = new ();

        // Convert constraints
        for(Int32 i = 1; i < transposed.Count; i++)
        {
            Dictionary<String, Double> dict = new ();

            dict.Add ("result", transposed[i].First ());

            for(Int32 j = 1; j < transposed[i].Count; j++)
            {
                dict.Add (variables[j], transposed[i][j]);
            }

            result.Add (dict);
        }

        // Convert objective funtion
        Dictionary<String, Double> lastRow = new ();

        lastRow.Add ("result", transposed[0].First ());
        for(Int32 j = 1; j < transposed[0].Count; j++)
        {
            lastRow.Add (variables[j], transposed[0][j]);
        }

        result.Add (lastRow);

        return result;
    }

    private static List<Dictionary<String, Double>> AddSlackVariables(
        List<Dictionary<String, Double>> dict)
    {
        for(Int32 i = 0; i < dict.Count; i++)
        {
            for(Int32 j = 0; j < dict.Count - 1; j++)
            {
                var value = i == j ? 1 : 0;
                dict[i].Add ($"s{j}", value);
            }
        }

        return dict;
    }
}
