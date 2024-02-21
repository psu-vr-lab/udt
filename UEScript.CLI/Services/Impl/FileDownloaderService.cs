using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services.Impl;

public class FileDownloaderService(IHttpClientFactory _httpClientFactory) : IFileDownloaderService
{
    public async Task<Result<Stream, CommandError>> DownloadFileFromUrl(string url)
    {
        var httpClient = _httpClientFactory.CreateClient();

        try
        {
            var responseMessage = await httpClient.GetStreamAsync(url);
            
            return Result<Stream, CommandError>.Ok(responseMessage);
        }
        catch (Exception e)
        {
            return Result<Stream, CommandError>.Error(new CommandError(e.Message));
        }
    }
}