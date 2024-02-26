using Microsoft.Extensions.Logging;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Writers;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UEScript.CLI.Commands;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services.Impl
{
    public class ArchiveService(ILogger<ArchiveExtractor> logger) : IArchiveService
    {
        Result<string, CommandError> ProcessArchive<T>(T archive, string sourcePath, string destinationPath)
        where T : IWritableArchive
        {
            try
            {
                archive.AddAllFromDirectory(sourcePath);
                archive.SaveTo(destinationPath, new WriterOptions((archive is TarArchive) ? CompressionType.None : CompressionType.Deflate));

                return Result<string, CommandError>.Ok("Directory was successfully archived to " + destinationPath);
            }
            catch (Exception e)
            {
                return Result<string, CommandError>.Error(new CommandError($"Error: " + e.Message));
            }
        }

        delegate IWritableArchive ArchiveCreator();
        static Dictionary<string, ArchiveCreator> archives = new Dictionary<string, ArchiveCreator> {
            {".zip", ZipArchive.Create },
            {".tar", TarArchive.Create },
            {".gz", GZipArchive.Create },
        };

        public Result<string, CommandError> Archive(string sourcePath, FileInfo destination)
        {
            if (!archives.ContainsKey(destination.Extension))
                return Result<string, CommandError>.Error(new CommandError("Unknown archive type"));
            var archive = archives[destination.Extension]();
            return ProcessArchive(archive, sourcePath, destination.FullName);
        }
    }
}
