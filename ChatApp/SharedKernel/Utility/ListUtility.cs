namespace SharedKernel.Utility;

public static class ListUtility
{
    public static bool HasDuplicates<T>(List<T> list)
    {
        return list.Count != list.Distinct().Count();
    }
}
