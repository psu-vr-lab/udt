using Microsoft.Extensions.Logging;
using UEScript.CLI.Commands.Engine.Add;
using UEScript.CLI.Services;
using UEScript.Utils.Results;

namespace UEScript.CLI.Commands.Engine.Install;

public static class InstallCommand
{
    public static async Task<Result<string, CommandError>> Execute
        (string name, FileInfo filePath, string url, ILogger logger,
            IFileDownloaderService fileDownloaderService, IUnrealEngineAssociationRepository repository)
    {
        logger.LogTrace("Install command start execution...");
        
        var directory = filePath.Directory;
        
        logger.LogInformation("Starting download engine zip from {url}...", url);
        
        var downloadResult = await fileDownloaderService.DownloadFile(url, directory);

        if (!downloadResult.IsSuccess)
        {
            return Result<string, CommandError>.Error(downloadResult);
        }
        
        logger.LogInformation("Download success");
        
        return AddCommand.Execute(name, filePath, true, repository, logger);;
    }
}