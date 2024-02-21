using System.IO.Compression;
using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services.Impl;

public class FileExtractor : IFileExtractor
{
    public Result<string, CommandError> ExtractStreamToDirectory(Stream stream, DirectoryInfo directory)
    {
        try
        {
            using var zip = new ZipArchive(stream);
            zip.ExtractToDirectory(directory.FullName);
            
            return Result<string, CommandError>.Ok($"Extracted files to {directory.FullName}");
        }
        catch (Exception e)
        {
            return Result<string, CommandError>.Error(new CommandError(e.Message));
        }
    }
}