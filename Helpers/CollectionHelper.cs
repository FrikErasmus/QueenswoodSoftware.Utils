namespace QueenswoodSoftware.Utils.Helpers;

public static class CollectionHelper
{
    public static IEnumerable<List<T>> Chunk<T>(this IEnumerable<T> source, int size)
    {
        var list = source.ToList();
        for (var i = 0; i < list.Count; i += size)
            yield return list.GetRange(i, Math.Min(size, list.Count - i));
    }

    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
    {
        var seen = new HashSet<TKey>();
        foreach (var item in source)
            if (seen.Add(keySelector(item)))
                yield return item;
    }

    /// <summary>
    /// Flattens a sequence of sequences into a single sequence.
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) =>
        source.SelectMany(x => x);

    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) =>
        source is null || !source.Any();

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source) action(item);
    }

    public static List<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();
        var rng = new Random();
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }

    public static Dictionary<TKey, TValue> ToDictionarySafe<T, TKey, TValue>(
        this IEnumerable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TValue> valueSelector) where TKey : notnull
    {
        var dict = new Dictionary<TKey, TValue>();
        foreach (var item in source)
        {
            var key = keySelector(item);
            if (!dict.ContainsKey(key))
                dict[key] = valueSelector(item);
        }
        return dict;
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class =>
        source.Where(x => x is not null)!;

    public static (List<T> Matches, List<T> NonMatches) Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        var matches = new List<T>();
        var nonMatches = new List<T>();
        foreach (var item in source)
            (predicate(item) ? matches : nonMatches).Add(item);
        return (matches, nonMatches);
    }
}