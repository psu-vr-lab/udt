using Microsoft.Extensions.Logging;
using UEScript.CLI.Commands.Engine.Add;
using UEScript.CLI.Services;
using UEScript.Utils.Results;

namespace UEScript.CLI.Commands.Engine.Install;

public static class InstallCommand
{
    public static async Task<Result<string, CommandError>> Execute(
        string name,
        FileInfo filePath,
        bool isDefault,
        string url, 
        ILogger logger,
        IFileDownloaderService fileDownloaderService, 
        IUnrealEngineAssociationRepository repository)
    {
        logger.LogTrace("Install command start execution...");
        
        var directory = filePath.Directory;

        if (directory is null)
        {
            // @Incomplete: Add typed error.
            return new CommandError($"Invalid directory '{filePath}'");
        }
        
        logger.LogInformation($"Starting download engine zip from '{url}'...");
        
        var downloadResult = await fileDownloaderService.DownloadFile(url, directory);

        if (!downloadResult.IsSuccess)
        {
            return Result<string, CommandError>.Error(downloadResult);
        }
        
        logger.LogInformation($"Downloaded engine from '{url}'");
        
        return AddCommand.Execute(name, filePath, isDefault, repository, logger);;
    }
}