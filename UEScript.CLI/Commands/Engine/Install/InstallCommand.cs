using System.Net;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Commands.Engine.Add;
using UEScript.CLI.Common;
using UEScript.CLI.Services;
using UEScript.Utils.Extensions;
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
        IFileExtractor fileExtractor,
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
        
        var downloadTask = DownloadFile(url, filePath, fileDownloaderService);
            
        var downloadResult = downloadTask.Result;
        
        if (downloadResult is null || !downloadResult.IsSuccess)
        {
            return Result<string, CommandError>.Error(downloadResult ?? new CommandError("Failed to download engine"));
        }
        
        logger.LogInformation($"Starting extract engine zip from '{url}'...");
        
        var fileExtractorResult = await ExtractDownloadFile(fileExtractor, filePath, directory);
        
        if (fileExtractorResult is null || !fileExtractorResult.IsSuccess)
        {
            return Result<string, CommandError>.Error(fileExtractorResult ?? new CommandError("Failed to extract engine"));
        }
        
        logger.LogResult(fileExtractorResult);
        
        return AddCommand.Execute(name, filePath, isDefault, repository, logger);;
    }
    
    private static async Task<Result<string, CommandError>?> DownloadFile(string url, FileInfo folder,
        IFileDownloaderService fileDownloaderService)
    {
        var downloadResult = default(Result<string, CommandError>);

        await AnsiConsoleUtils.WrapTaskAroundProgressBar("Downloading: ", async (ctx) =>
        {
            downloadResult = fileDownloaderService.DownloadFileFromUrl(url, folder, Action);

            void Action(DownloadProgressChangedEventArgs value)
            {
                ctx.Increment(value.ProgressPercentage);
            }

            while (!ctx.IsFinished)
            {
                await Task.Delay(300);
            }
        });
        
        return downloadResult;
    }
    
    private static async Task<Result<string, CommandError>?> ExtractDownloadFile(IFileExtractor fileExtractor,FileInfo fileInfo, DirectoryInfo directory)
    {
        var fileExtractorResult = default(Result<string, CommandError>);
        await using var stream = new FileStream(fileInfo.FullName, FileMode.Open);
        
        await AnsiConsoleUtils.WrapTaskAroundProgressBar("Extracting: ", () =>
        {
            fileExtractorResult = fileExtractor.ExtractStreamToDirectory(stream, directory);
        });

        return fileExtractorResult;
    }
}