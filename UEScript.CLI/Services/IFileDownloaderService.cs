using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services;

public interface IFileDownloaderService
{
    public Task<Result<string, CommandError>> DownloadFile(string url, DirectoryInfo filePath);
}