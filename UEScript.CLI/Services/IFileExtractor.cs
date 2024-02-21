using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services;

public interface IFileExtractor
{
    public Result<string, CommandError> ExtractStreamToDirectory(Stream stream, DirectoryInfo directory);
}