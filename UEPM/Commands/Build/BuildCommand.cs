using Microsoft.Extensions.Configuration;

namespace Ueco.Commands.Build;

public class BuildCommand(IConfiguration configuration)
{
    public void Execute(FileInfo fileInfo)
    {
        Console.WriteLine($"Hello World!, {configuration["Build:Name"]}");
        Console.WriteLine($"Hello World!, {fileInfo.Name}");
    }
}