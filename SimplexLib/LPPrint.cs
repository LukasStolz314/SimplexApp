using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexLib;
public static class LPPrint
{
    public static String FilePath = $"log_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.txt";

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

        File.AppendAllLinesAsync (FilePath, result);
        foreach(var line in result)
            Console.WriteLine(line);
    }

    public static void PrintMeta(Int32 iteration, List<Double> quotients,
        Double pivotValue, (String, Int32) pivotPosition)
    {
        List<String> result = new () { $"Iteration: {iteration + 1}" };

        String quotientLine = $"Quotients: ";
        foreach(var quotient in quotients)
            quotientLine += $"{quotient.ToString(CultureInfo.InvariantCulture)} | ";

        result.Add(quotientLine);

        result.Add($"Pivot-Element: {pivotValue.ToString(CultureInfo.InvariantCulture)}");
        result.Add ($"Pivot-Position: Variable: {pivotPosition.Item1} | Row: {pivotPosition.Item2}");

        File.AppendAllLinesAsync (FilePath, result);
        foreach(var line in result)
            Console.WriteLine(line);
    }

    public static void Print(String text)
    {
        File.AppendAllLinesAsync(FilePath, new List<String>() { text });
        Console.WriteLine(text);
    }

    public static void PrintResult(Dictionary<String, Double> results)
    {
        List<String> result = new ();
        result.Add ($"Result: {results.First(x => x.Key.Equals("Result: ")).Value}");
        foreach(var item in results)
        {
            if(item.Key != "Result: ")
                result.Add($"{item.Key}: {item.Value}");
        }

        File.AppendAllLinesAsync (FilePath, result);
        foreach(var line in result)
            Console.WriteLine(line);
    }
}
