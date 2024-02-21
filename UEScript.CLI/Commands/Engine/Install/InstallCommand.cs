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
        
        var downloadResult = await DownloadFile(url, fileDownloaderService);
        
        if (downloadResult is null || !downloadResult.IsSuccess)
        {
            return Result<string, CommandError>.Error(downloadResult ?? new CommandError("Failed to download engine"));
        }
        
        logger.LogInformation($"Starting extract engine zip from '{url}'...");
        
        var fileExtractorResult = await ExtractDownloadFile(fileExtractor, downloadResult, directory);
        
        if (fileExtractorResult is null || !fileExtractorResult.IsSuccess)
        {
            return Result<string, CommandError>.Error(fileExtractorResult ?? new CommandError("Failed to extract engine"));
        }
        
        logger.LogResult(fileExtractorResult);
        
        return AddCommand.Execute(name, filePath, isDefault, repository, logger);;
    }
    
    private static async Task<Result<Stream, CommandError>?> DownloadFile(string url, IFileDownloaderService fileDownloaderService)
    {
        var downloadResult = default(Result<Stream, CommandError>);

        await AnsiConsoleUtils.WrapTaskAroundProgressBar("Downloading: ",async () =>
        { 
            downloadResult = await fileDownloaderService.DownloadFileFromUrl(url);
        });
        
        return downloadResult;
    }
    
    private static async Task<Result<string, CommandError>?> ExtractDownloadFile(IFileExtractor fileExtractor, Result<Stream, CommandError> downloadResult, DirectoryInfo directory)
    {
        var fileExtractorResult = default(Result<string, CommandError>);
        var downloadStream = downloadResult.GetValue();

        await AnsiConsoleUtils.WrapTaskAroundProgressBar("Extracting: ", () =>
        {
            fileExtractorResult = fileExtractor.ExtractStreamToDirectory(downloadStream!, directory);
        });

        return fileExtractorResult;
    }
}