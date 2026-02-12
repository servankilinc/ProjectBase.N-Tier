using Core.Utils.Datatable;
using Core.Utils.DynamicQuery;

namespace Core.BaseRequestModels;

public class DynamicDatatableRequest : DatatableRequest
{
    public Filter? Filter { get; set; }
    public IEnumerable<Sort>? Sorts { get; set; }
}