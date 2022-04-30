namespace SimplexLib;

public class LPArrange
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
                table.ArrayTable[i, j] = constraints[i].valuePairs[j].value;

                // Set table column header names
                table.AddColumnName(constraints[i].valuePairs[j].varName);
            }

            // Set table row and column header names
            table.AddRowName(($"s{i}"));
            table.AddColumnName(($"s{i}"));

            // Set corresponding slack variable of constrait
            table.ArrayTable[i, variableCount + i] = 1;
        }

        // Insert objective function into bottom table row
        Int32 lastRowIndex = table.ArrayTable.GetLength(0) - 1;
        for (int i = 0; i < objFunction.valuePairs.Count; i++)
            table.ArrayTable[lastRowIndex, i] = objFunction.valuePairs[i].value;

        return table;
    }
}
