namespace SimplexLib;

public class Table
{
    /* Table example
     * 16  6   0 0 252
     * 4   12  0 0 168
     * 150 100 0 0 result
     */

    public Double[,] Array { get; init; }
    public List<String> ColumnVarNames { get; private set; } = new();
    public List<String> RowVarNames { get; private set; } = new();

    public Table(Int32 columnCount, Int32 rowCount)
    {
        Array = new Double[columnCount, rowCount];
    }

    public void AddColumnName(String value)
    {
        if(!ColumnVarNames.Contains(value))
            ColumnVarNames.Add(value);
    }

    public void AddRowName(String value)
    {
        if (!RowVarNames.Contains(value))
            RowVarNames.Add(value);
    }

    public Double[] GetRow(Int32 row)
    {
        return Enumerable.Range(0, Array.GetLength(1))
                .Select(x => Array[row, x])
                .ToArray();
    }

    public void SwitchColumnWithRowHeader(Int32 column, Int32 row)
    {
        String temp = ColumnVarNames[column];
        ColumnVarNames[column] = RowVarNames[row];
        RowVarNames[row] = temp;
    }
}
