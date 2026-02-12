using Core.Utils.DynamicQuery;
using Core.Utils.Pagination;

namespace Core.BaseRequestModels;

public class DynamicPaginationRequest : PaginationRequest
{
    public Filter? Filter { get; set; }
    public IEnumerable<Sort>? Sorts { get; set; }
}
