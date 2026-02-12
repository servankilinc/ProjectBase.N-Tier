using Core.Utils.DynamicQuery;

namespace Core.BaseRequestModels;

public class DynamicRequest
{
    public Filter? Filter { get; set; }
    public IEnumerable<Sort>? Sorts { get; set; }
}
