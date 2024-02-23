using System.Net;
using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services.Impl;

public class FileDownloaderService(IHttpClientFactory _httpClientFactory) : IFileDownloaderService
{
    [Obsolete("Obsolete")]
    public Result<string, CommandError> DownloadFileFromUrl(string url, FileInfo folder, Action<DownloadProgressChangedEventArgs> action)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(url);
        using var webClient = new WebClient();
        try
        {
            webClient.DownloadProgressChanged += ((sender, args) => action(args));
            webClient.DownloadFileAsync(new Uri(url), folder.FullName);
            
            return Result<string, CommandError>.Ok("Download completed");
        }
        catch (Exception e)
        {
            return Result<string, CommandError>.Error(new CommandError("Failed to download engine"));
        }
    }
}