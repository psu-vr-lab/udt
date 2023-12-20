using Ueco.Utils.Results;

namespace Ueco.Commands.Engine.List;

public class ListCommandError : Error
{
    private ListCommandError(string message, Exception? exception = null) : base(message, exception)
    {
    }
    
    public static ListCommandError NoEngineAssociations()
        => new ListCommandError("You don't have any engines installed. Use `ueco engine add` to add one.");
}