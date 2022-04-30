namespace SimplexLib;

public enum ProblemType
{
    Minimize,
    Maximize
}

public record ObjFunction(ProblemType problemType, List<(Int32 value, String varName)> valuePairs);