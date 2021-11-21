namespace HotTowel.Web.Results
{
    public class QueryResult<T> : IQueryResult
    {
        public T Data { get; set; }
        public IResult Status { get; set; }
    }
}