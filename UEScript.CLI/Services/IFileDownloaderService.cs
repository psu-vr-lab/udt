using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services;

public interface IFileDownloaderService
{
    public Task<Result<Stream, CommandError>> DownloadFileFromUrl(string url);
}