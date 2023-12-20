using Ueco.Utils.Results;

namespace Ueco.Commands.Engine.Delete;

public class DeleteCommandError : Error
{
    private DeleteCommandError(string message, Exception? exception = null) : base(message, exception)
    {
    }
    
    public static DeleteCommandError IndexOutOfRange(int index, int maxIndex)
        => new DeleteCommandError($"Index out of range: {index}, max index: {maxIndex}");
}