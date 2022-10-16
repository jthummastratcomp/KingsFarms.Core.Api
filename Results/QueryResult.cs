namespace KingsFarms.Core.Api.Results;

public class QueryResult<T> : IQueryResult
{
    public T Data { get; set; }
    public IResult Status { get; set; }
}