using System.Linq.Dynamic.Core;

namespace Core.Utils.DynamicQuery;

public static class QueryableFilterExtension
{
    public static readonly HashSet<string> Logics = new(StringComparer.OrdinalIgnoreCase)
    {
        "and", "or"
    };

    public static readonly Dictionary<string, Func<string, int, string>> OperatorsWithValue = new(StringComparer.OrdinalIgnoreCase)
    {
        ["eq"] = (f, i) => $"np({f}) == @{i}",
        ["neq"] = (f, i) => $"np({f}) != @{i}",
        ["lt"] = (f, i) => $"np({f}) < @{i}",
        ["lte"] = (f, i) => $"np({f}) <= @{i}",
        ["gt"] = (f, i) => $"np({f}) > @{i}",
        ["gte"] = (f, i) => $"np({f}) >= @{i}",
        ["startswith"] = (f, i) => $"np({f}).StartsWith(@{i})",
        ["endswith"] = (f, i) => $"np({f}).EndsWith(@{i})",
        ["contains"] = (f, i) => $"np({f}).Contains(@{i})",
        ["doesnotcontain"] = (f, i) => $"!np({f}).Contains(@{i})"
    };

    public static readonly Dictionary<string, Func<string, string>> OperatorsWithoutValue = new(StringComparer.OrdinalIgnoreCase)
    {
        ["isnull"] = f => $"np({f}) == null",
        ["isnotnull"] = f => $"np({f}) != null"
    };

    public static IQueryable<T> ToFilter<T>(this IQueryable<T> query, Filter filter)
    {
        if (filter is null) return query;

        var parameters = new List<object>();
        var where = BuildExpression(filter, parameters);

        return string.IsNullOrWhiteSpace(where) ? query : query.Where(where, parameters.ToArray());
    }

    private static void Validate(Filter filter)
    {
        if (filter.Operator == "base") return;

        if (string.IsNullOrWhiteSpace(filter.Field))
            throw new ArgumentException("Empty field for dynamic filter");

        if (!OperatorsWithValue.ContainsKey(filter.Operator!) && !OperatorsWithoutValue.ContainsKey(filter.Operator!))
            throw new ArgumentException($"Invalid opreator type for dynamic filter, operator: {filter.Operator}");

        if (filter.Value is null && OperatorsWithValue.ContainsKey(filter.Operator!))
            throw new ArgumentException($"Value required for operator: {filter.Operator}");

        if (!string.IsNullOrWhiteSpace(filter.Logic) && !Logics.Contains(filter.Logic))
            throw new ArgumentException($"Invalid logic type for dynamic filter, logic: {filter.Logic}");
    }

    private static string BuildExpression(Filter filter, IList<object> parameters)
    {
        Validate(filter);

        var expressions = new List<string>();

        if (filter.Operator != "base")
        {
            if (OperatorsWithValue.ContainsKey(filter.Operator!))
            {
                var index = parameters.Count;
                parameters.Add(filter.Value!);
                expressions.Add(OperatorsWithValue[filter.Operator!](filter.Field!, index));
            }
            else
            {
                expressions.Add(OperatorsWithoutValue[filter.Operator!](filter.Field!));
            }
        }

        if (filter.Filters?.Any() == true)
        {
            var childExpressions = filter.Filters
                .Select(f => BuildExpression(f, parameters))
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToList();

            if (childExpressions.Any())
            {
                expressions.Add($"({string.Join($" {filter.Logic} ", childExpressions)})");
            }
        }

        return expressions.Any() ? string.Join($" {filter.Logic} ", expressions) : string.Empty;
    }
}