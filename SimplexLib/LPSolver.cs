namespace SimplexLib;

public class LPSolver
{
    public Table Arrange(ObjFunction objFunction, List<Constraint> constraints)
    {
        Int32 slackCount = constraints.Count;
        Int32 variableCount = objFunction.valuePairs.Count;

        // +1 each to save result and objFunction
        Table table = new Table(slackCount + 1, variableCount + slackCount + 1);

        for(int i = 0; i < constraints.Count; i++)
        {
            for (int j = 0 ; j < constraints[i].valuePairs.Count ; j++)
            {
                // Insert constrait values values into the table
                table.Array[i, j] = constraints[i].valuePairs[j].value;

                // Set table column header names
                table.AddColumnName(constraints[i].valuePairs[j].varName);
            }

            // Set table row and column header names
            table.AddRowName(($"s{i}"));
            table.AddColumnName(($"s{i}"));

            // Set corresponding slack variable of constrait
            table.Array[i, variableCount + i] = 1;

            table.Array[i, table.Array.GetLength(1) - 1] = constraints[i].result;
        }

        // Insert objective function into bottom table row
        Int32 lastRowIndex = table.Array.GetLength(0) - 1;
        for (int i = 0; i < objFunction.valuePairs.Count; i++)
            table.Array[lastRowIndex, i] = objFunction.valuePairs[i].value * -1;

        return table;
    }

    public List<(String, Double)> Solve(Table table)
    {
        // Useful information
        Int32 highestRow = table.Array.GetLength(0) - 1;
        Int32 highestColumn = table.Array.GetLength(1) - 1;

        while(table.GetRow(highestRow).Any(x => x < 0))
        {
            // Get column index for lowest object function value
            List<Double> row = table.GetRow(highestRow).ToList();
            Int32 lowestObjFncIndex = row.IndexOf(row.Min());

            // Calculate quotients for every value with calculated column index
            Dictionary<Int32, Double> quotients = new();
            for (int i = 0; i < highestRow; i++)
            {
                Double quotient = table.Array[i, highestColumn] / table.Array[i, lowestObjFncIndex];
                quotients.Add(i, quotient);
            }

            // Determine lowest quotient and write it to the result field
            KeyValuePair<Int32, Double> lowestQuotient = quotients.MinBy(x => x.Value);
            table.Array[lowestQuotient.Key, highestColumn] = lowestQuotient.Value;

            // Divide lowest quotient row by the record with
            // lowest quotient and lowest object function value (Create unit vector)
            double rowQuotient = table.Array[lowestQuotient.Key, lowestObjFncIndex];
            for (int i = 0; i < highestColumn; i++)
                table.Array[lowestQuotient.Key, i] /= rowQuotient;

            // Switch variable names
            table.SwitchColumnWithRowHeader(lowestObjFncIndex, lowestQuotient.Key);

            // Create unit vector
            for (int i = 0; i <= highestRow; i++)
            {
                if (i != lowestQuotient.Key)
                {
                    // Value of record in current row and lowest object function value column
                    Double rowValue = table.Array[i, lowestObjFncIndex];
                    for (int j = 0; j <= highestColumn; j++)
                    {
                        double recordValue = table.Array[i, j];
                        double lowestQuotientRecordValue = table.Array[lowestQuotient.Key, j];
                        table.Array[i, j] = recordValue - rowValue * lowestQuotientRecordValue;
                    }
                }
            }
        }

        List<(String, Double)> result = new();
        for(int i = 0; i < highestRow; i++)
        {
            (String, Double) tuple = new(table.RowVarNames[i],
                table.Array[i, highestColumn]);

            result.Add(tuple);
        }

        result.Add(new("Result", table.Array[highestRow, highestColumn]));

        return result;
    }
}
