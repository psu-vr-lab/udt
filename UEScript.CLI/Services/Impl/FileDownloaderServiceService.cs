using System.IO.Compression;
using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services.Impl;

public class FileDownloaderServiceService : IFileDownloaderService
{
    public async Task<Result<string, CommandError>> DownloadFile(string url, DirectoryInfo filePath)
    {
        try
        {
            var uri = new Uri(url);
            
            using var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetStreamAsync(uri);
            
            using var zip = new ZipArchive(responseMessage);
            zip.ExtractToDirectory(filePath.ToString());
        }
        catch (Exception ex)
        {
            return Result<string, CommandError>.Error(new CommandError(ex.Message));
        }
        
        return Result<string, CommandError>.Ok("File downloaded");
    }
}