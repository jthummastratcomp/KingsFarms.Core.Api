namespace KingsFarms.Core.Api.Results;

public class CommandResult : ICommandResult
{
    public IResult Status { get; set; }
}