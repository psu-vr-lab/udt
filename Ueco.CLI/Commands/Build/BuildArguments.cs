using System.CommandLine;

namespace Ueco.Commands.Build;

public class BuildArguments(string fileInfo)
{
    public string FileInfo { get; set; } = fileInfo;
    
    public static CliArgument[] GetArguments()
    {
        var fileArgument = new CliArgument<string>("file")
        {
            Description = "File to build",
        };
        
        return new CliArgument[] { fileArgument };
    }
}