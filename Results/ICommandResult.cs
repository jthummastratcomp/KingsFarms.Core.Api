namespace KingsFarms.Core.Api.Results
{
    public interface ICommandResult
    {
        IResult Status { get; set; }
    }
}