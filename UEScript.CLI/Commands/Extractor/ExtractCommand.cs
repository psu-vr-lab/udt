using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UEScript.CLI.Services;
using UEScript.Utils.Extensions;
using UEScript.Utils.Results;

namespace UEScript.CLI.Commands.ArchiveExctractor;

public static class ExtractCommand
{

    public static Result<string, CommandError> Execute(FileInfo file, FileInfo destinationPath, IArchiveExtractor archiveExtractor, ILogger logger)
    {
        logger.LogTrace("Extract command start execution...");

        if (file is null)
            return new CommandError("File is null");
        if (!file.Exists)
            return CommandError.FileNotFound(file);

        if (destinationPath is null)
            return new CommandError("Destination path is null");
        var dir = destinationPath.Directory;
        if (dir is null || !dir.Exists)
            return CommandError.PathIsNotDirectory(destinationPath.FullName);

        var result = archiveExtractor.Extract(file, dir.FullName);
        if (!result.IsSuccess)
            return result;
        if (file.Name.EndsWith(".tar.gz"))
        {
            logger.LogResult(result);
            logger.LogInformation("Trying to extract .tar archive from .tar.gz archive");
            result = archiveExtractor.Extract(new FileInfo(dir.FullName + "/" + file.Name.Substring(0, file.Name.Length - 3)), dir.FullName);
        }
        return result;
    }
}
