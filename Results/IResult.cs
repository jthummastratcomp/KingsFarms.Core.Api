namespace KingsFarms.Core.Api.Results;

public interface IResult
{
    int Id { get; }
    string Message { get; }
    object Data { get; }
    ResultType ResultType { get; }
}