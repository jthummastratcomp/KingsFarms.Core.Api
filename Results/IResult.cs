namespace HotTowel.Web.Results
{
    public interface IResult
    {
        int Id { get; }
        string Message { get; }
        object Data { get; }
        ResultType ResultType { get; }
    }
}