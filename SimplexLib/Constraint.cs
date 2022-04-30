namespace SimplexLib;

public record Constraint (List<(Int32 value, String varName)> valuePairs, Int32 result);
