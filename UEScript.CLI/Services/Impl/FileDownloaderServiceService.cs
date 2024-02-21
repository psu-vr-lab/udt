using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services.Impl;

public class FileDownloaderServiceService : IFileDownloaderService
{
    public async Task<Result<string, CommandError>> DownloadFile(string url, DirectoryInfo filePath, ILogger logger)
    {
        try
        {
            logger.LogInformation("Downloading file from {url}...", url);
            var responseMessage = default(Stream);
            
            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                var gettingReadyTask = ctx.AddTask("[cyan]Downloading progress:[/]");
                var uri = new Uri(url);
                using var httpClient = new HttpClient();
                responseMessage = await httpClient.GetStreamAsync(uri);
                while (!ctx.IsFinished)
                {
                    await Task.Delay(300);
                    gettingReadyTask.Increment(10.5);
                }
            });
            
            logger.LogInformation("File downloaded");
            
            logger.LogInformation("Extracting file from archive...");
            
            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                var gettingReadyTask = ctx.AddTask("[cyan]Unzipping progress:[/]");
                using var zip = new ZipArchive(responseMessage);
                zip.ExtractToDirectory(filePath.ToString());
                while (!ctx.IsFinished)
                {
                    await Task.Delay(300);
                    gettingReadyTask.Increment(10.5);
                }
            });
            
            logger.LogInformation("File extracted");
        }
        catch (Exception ex)
        {
            return Result<string, CommandError>.Error(new CommandError(ex.Message));
        }
        
        return Result<string, CommandError>.Ok("File downloaded");
    }
}