namespace SimplexLib;

public class LPSolver
{
    private List<Dictionary<String, Double>> _list = new ();

    public LPSolver(List<Dictionary<String, Double>> list)
    {
        _list = list;
    }

    public Double Solve()
    {
        Boolean win = IsWong();
        Int32 iteration = 0;
        while(win)
        {
            var pivotVariable = GetPivotVariable();

            var pivotRow = GetPivotRow(pivotVariable);

            UnifyPivotRow(pivotRow, pivotVariable);

            UnifyOtherRows(pivotRow, pivotVariable);

            win = IsWong();

            iteration++;
            Console.WriteLine ($"Iteration: {iteration}");
            LPPrint.Print (_list);
        }

        return _list.Last()["result"] * -1;
    }

    private String GetPivotVariable()
    {
        return _list.Last().Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
    }

    private Int32 GetPivotRow(String variable)
    {
        Dictionary<Int32, Double> quotients = new ();

        for(Int32 i = 0; i < _list.Count - 1; i++)
        {
            if (_list[i][variable] == 0)
                continue;

            Double quotient = _list[i]["result"] / _list[i][variable];            

            if (quotient <= 0)
                continue;

            quotients.Add(i, quotient);
        }

        return quotients.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;
    }

    private void UnifyPivotRow(Int32 row, String variable)
    {
        Double divider = _list[row][variable];

        Dictionary<String, Double> newRow = new ();

        foreach(var item in _list[row])
        {
            newRow.Add(item.Key, item.Value / divider);
        }

        _list[row] = newRow;
    }

    private void UnifyOtherRows(Int32 pivotRow, String variable)
    {
        List<Dictionary<String, Double>> result = new ();
        foreach(var row in _list)
        {
            if (_list[pivotRow] == row)
            {
                result.Add(row);
                continue;
            }

            Dictionary<String, Double> newRow = new ();

            Double multiplier = row[variable];
            foreach(var item in row)
            {
                var value = item.Value - multiplier * _list[pivotRow][item.Key];
                newRow.Add (item.Key, value);
            }

            result.Add (newRow);
        }

        _list = result;
    }

    private Boolean IsWong() => _list.Last ().Values.Any (x => x > 0);
}