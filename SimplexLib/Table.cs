namespace SimplexLib;

public class Table
{
    public Int32[,] ArrayTable { get; init; }
    public List<String> ColumnVarName { get; private set; } = new();
    public List<String> RowVarName { get; private set; } = new();

    public Table(Int32 columnCount, Int32 rowCount)
    {
        ArrayTable = new Int32[columnCount, rowCount];
    }

    public void AddColumnName(String value)
    {
        if(!ColumnVarName.Contains(value))
            ColumnVarName.Add(value);
    }

    public void AddRowName(String value)
    {
        if (!RowVarName.Contains(value))
            RowVarName.Add(value);
    }
}
