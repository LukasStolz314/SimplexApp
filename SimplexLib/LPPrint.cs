using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexLib;
public static class LPPrint
{
    public static void Print(List<Dictionary<String, Double>> list)
    {
        for(Int32 i = 0; i < list.Count - 1; i++)
        {
            foreach(var entry in list[i])
            {
                if(!entry.Key.Equals("result"))
                    Console.Write($"{entry.Key}:{entry.Value}  ");
            }

            Console.Write($"result:{list[i]["result"]} \r\n");
        }

        Console.WriteLine ("-----------------------------");

        foreach(var entry in list.Last())
        {
            if(!entry.Key.Equals("result"))
                Console.Write($"{entry.Key}:{entry.Value}  ");
        }

        Console.Write($"result:{list.Last()["result"]} \r\n");

        Console.WriteLine ("\r\n#####################\r\n");
    }
}
