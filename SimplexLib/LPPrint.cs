using System.Globalization;

namespace SimplexLib;
public static class LPPrint
{
    public static String FilePath = $"log_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.txt";

    public static void PrintInitialTable(List<Dictionary<String, Double>> list)
    {
        List<Dictionary<String, Double>> tempList = new(list);
        var objFunction = tempList.FirstOrDefault () ?? new();
        tempList.RemoveAt(0);

        tempList.Add (objFunction);
        PrintTable (tempList);
    }

    public static void PrintTable(List<Dictionary<String, Double>> list)
    {
        List<String> result = new ();
        for(Int32 i = 0; i < list.Count - 1; i++)
        {
            String constraints = "";
            foreach(var entry in list[i])
            {
                if (!entry.Key.Equals ("result"))
                    constraints += $"{entry.Key}:{entry.Value.ToString (CultureInfo.InvariantCulture)}  ";
            }

            result.Add (constraints + $"result:{list[i]["result"]}");
        }

        result.Add ("-----------------------------");

        String objFunction = "";
        foreach(var entry in list.Last())
        {
            if(!entry.Key.Equals("result"))
                objFunction += $"{entry.Key}:{entry.Value.ToString(CultureInfo.InvariantCulture)}  ";
        }

        result.Add(objFunction + $"result:" +
            $"{list.Last ()["result"].ToString (CultureInfo.InvariantCulture)}");

        result.Add ("\r\n#####################\r\n");

        File.AppendAllLines (FilePath, result);
        foreach(var line in result)
            Console.WriteLine(line);
    }

    public static void PrintMeta(Int32 iteration, List<Double> quotients,
        Double pivotValue, (String, Int32) pivotPosition, Double maxObjValue)
    {
        List<String> result = new () { $"Iteration: {iteration + 1} \r\n"};

        result.Add ($"Maximal value in: {pivotPosition.Item1} => {maxObjValue.ToString(CultureInfo.InvariantCulture)}");

        String quotientLine = $"Quotients: ";
        foreach(var quotient in quotients)
            quotientLine += $"{quotient.ToString(CultureInfo.InvariantCulture)} | ";

        result.Add(quotientLine);
        result.Add ("");

        result.Add($"Pivot-Value: {pivotValue.ToString(CultureInfo.InvariantCulture)}");
        result.Add ($"Pivot-Position: Variable: {pivotPosition.Item1} | Row: {pivotPosition.Item2 + 1}");

        result.Add ("");

        File.AppendAllLines (FilePath, result);
        foreach(var line in result)
            Console.WriteLine(line);
    }

    public static void Print(String text)
    {
        File.AppendAllLines(FilePath, new List<String>() { text });
        Console.WriteLine(text);
    }

    public static void PrintResult(Dictionary<String, Double> results, TimeSpan timespan)
    {
        List<String> result = new () { $"Total time: {timespan.TotalSeconds.ToString(CultureInfo.InvariantCulture)} seconds" };
        result.Add ($"Result: {results.First(x => x.Key.Equals("Result: ")).Value.ToString(CultureInfo.InvariantCulture)}");


        foreach(var item in results)
        {
            if(item.Key != "Result: ")
                result.Add($"{item.Key}: {item.Value.ToString(CultureInfo.InvariantCulture)}");
        }

        File.AppendAllLines (FilePath, result);
        foreach(var line in result)
            Console.WriteLine(line);
    }
}
