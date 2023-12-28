namespace SharedKernel.Utility;

public static class ListUtility
{
    public static bool HasDuplicateGuids(List<Guid> list)
    {
        return list.GroupBy(g => g).Any(g => g.Count() > 1);
    }
}
