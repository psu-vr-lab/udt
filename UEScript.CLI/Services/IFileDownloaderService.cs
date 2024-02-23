using System.Net;
using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services;

public interface IFileDownloaderService
{
    public Result<string, CommandError> DownloadFileFromUrl(string url, FileInfo folder, Action<DownloadProgressChangedEventArgs> action);
}