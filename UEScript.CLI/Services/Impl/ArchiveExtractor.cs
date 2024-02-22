using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UEScript.Utils.Results;
using UEScript.CLI.Commands;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Archives.Rar;
using SharpCompress.Common.Tar;
using System.Formats.Tar;
using SharpCompress.Common.Zip;
using SharpCompress.Common.Rar;
using SharpCompress.Common.SevenZip;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.GZip;
using SharpCompress.Common.GZip;

namespace UEScript.CLI.Services.Impl;


public class ArchiveExtractor(ILogger<ArchiveExtractor> logger) : IArchiveExtractor
{
    Result<string, CommandError> ProcessArchive<T, TEntry, TVolume>(T archive, string destinationPath)
        where T : AbstractArchive<TEntry, TVolume> 
        where TEntry : IArchiveEntry
        where TVolume : IVolume
    {
        int lastPerc = 0;
        if (archive is SevenZipArchive)
            archive.CompressedBytesRead += (sender, e) =>
            {
                double progress = (double)e.CurrentFilePartCompressedBytesRead / archive.TotalSize * 100;
                if (progress >= lastPerc)
                {
                    lastPerc += 5;
                    logger.LogInformation($"Progress: {progress}%");
                }
            };
        else
        {
            long totalRead = 0;
            archive.EntryExtractionEnd += (sender, e) =>
            {
                totalRead += e.Item.CompressedSize;
                double progress = (double)totalRead / archive.TotalSize * 100;
                if (progress >= lastPerc)
                {
                    lastPerc += 5;
                    logger.LogInformation($"Progress: {progress}%");
                }
            };
        }

        foreach (var entry in archive.Entries)
            if (!entry.IsDirectory)
                entry.WriteToDirectory(destinationPath, new ExtractionOptions(){ Overwrite = true });

        return Result<string, CommandError>.Ok("Archive was successfully extracted to " + destinationPath);
    }
    public Result<string, CommandError> Extract(FileInfo file, string destinationPath)
    {
        // не придумал как сделать лучше

        if (file.Extension == ".zip")
            return ProcessArchive<ZipArchive, ZipArchiveEntry, ZipVolume>(ZipArchive.Open(file), destinationPath);
        if (file.Extension == ".rar")
            return ProcessArchive<RarArchive, RarArchiveEntry, RarVolume>(RarArchive.Open(file), destinationPath);
        if (file.Extension == ".7z")
            return ProcessArchive<SevenZipArchive, SevenZipArchiveEntry, SevenZipVolume>(SevenZipArchive.Open(file), destinationPath);
        if (file.Extension == ".tar")
            return ProcessArchive<TarArchive, TarArchiveEntry, TarVolume>(TarArchive.Open(file), destinationPath);
        if (file.Extension == ".gz")
            return ProcessArchive<GZipArchive, GZipArchiveEntry, GZipVolume>(GZipArchive.Open(file), destinationPath);

        return Result<string, CommandError>.Error(new CommandError("Unknown archive type"));
    }
    public Result<string, CommandError> ExtractFromStream(Stream stream, string extension, string destinationPath)
    {
        if (extension == ".zip")
            return ProcessArchive<ZipArchive, ZipArchiveEntry, ZipVolume>(ZipArchive.Open(stream), destinationPath);
        if (extension == ".rar")
            return ProcessArchive<RarArchive, RarArchiveEntry, RarVolume>(RarArchive.Open(stream), destinationPath);
        if (extension == ".7z")
            return ProcessArchive<SevenZipArchive, SevenZipArchiveEntry, SevenZipVolume>(SevenZipArchive.Open(stream), destinationPath);
        if (extension == ".tar")
            return ProcessArchive<TarArchive, TarArchiveEntry, TarVolume>(TarArchive.Open(stream), destinationPath);
        if (extension == ".gz")
            return ProcessArchive<GZipArchive, GZipArchiveEntry, GZipVolume>(GZipArchive.Open(stream), destinationPath);

        return Result<string, CommandError>.Error(new CommandError("Unknown archive type"));
    }

    public Result<string, CommandError> ExtractZipFromStream(Stream stream, string destinationPath)
    {
        return ExtractFromStream(stream, ".zip", destinationPath);
    }
}
