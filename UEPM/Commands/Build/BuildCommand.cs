using Microsoft.Extensions.Configuration;

namespace Ueco.Commands.Build;

public static class BuildCommand
{
    public static Task ExecuteAsync(FileInfo fileInfo, IConfiguration configuration)
    {
        var uprojectFile = fileInfo;
        
        if (fileInfo.Extension != ".uproject" && fileInfo.Directory is not null)
        {
            uprojectFile = fileInfo.Directory.GetFiles("*.uproject", SearchOption.TopDirectoryOnly).FirstOrDefault();
        }
        
        Console.WriteLine($"File: {uprojectFile?.FullName}");
        
        return Task.CompletedTask;
    }
}