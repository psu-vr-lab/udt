using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UEScript.Utils.Results;
using UEScript.CLI.Commands;
using System.Collections;
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
using System.Linq;
using SharpCompress;
using UEScript.CLI.Common;
using System.Net;
using Spectre.Console;

namespace UEScript.CLI.Services.Impl;


public class ArchiveExtractor(ILogger<ArchiveExtractor> logger) : IArchiveExtractor
{
    Result<string, CommandError> ProcessArchive<T, TEntry, TVolume>(T archive, string destinationPath, Action<double>? progressBarAction)
        where T : AbstractArchive<TEntry, TVolume> 
        where TEntry : IArchiveEntry
        where TVolume : IVolume
    {
        try
        {
            int lastPerc = 0;
            long totalRead = 0;
            if (progressBarAction is not null)
                progressBarAction(0);

            if (archive is SevenZipArchive)
                archive.CompressedBytesRead += (sender, e) =>
                {
                    double progress = (double)e.CurrentFilePartCompressedBytesRead / archive.TotalSize * 100;
                    if (progressBarAction is not null)
                        progressBarAction(progress);
                };
            else if (archive is not GZipArchive)
                archive.EntryExtractionEnd += (sender, e) =>
                {
                    totalRead += e.Item.CompressedSize;
                    double progress = (double)totalRead / archive.TotalSize * 100;
                    if (progressBarAction is not null)
                        progressBarAction(progress);
                };

            archive.Entries
                .Where(e => !e.IsDirectory)
                .ForEach(e => e.WriteToDirectory(destinationPath, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true }));

            if (progressBarAction is not null)
                progressBarAction(100);

            return Result<string, CommandError>.Ok("Archive was successfully extracted to " + destinationPath);
        }
        catch (Exception e)
        {
            return Result<string, CommandError>.Error(new CommandError($"Error: " + e.Message));
        }
    }

    public Result<string, CommandError> ExtractSelector(string extension, string destinationPath, FileInfo? file, bool isFile = true, Stream? stream = null, Action<double>? progressBarAction = null)
    {
        if (isFile && file is null)
            return Result<string, CommandError>.Error(new CommandError("File archive source is not provided (equals null)"));

        if (!isFile && stream is null)
            return Result<string, CommandError>.Error(new CommandError("Stream archive source is not provided (equals null)"));
        
        switch (extension)
        {
            case ".zip":
                return ProcessArchive<ZipArchive, ZipArchiveEntry, ZipVolume>(isFile ? ZipArchive.Open(file!) : ZipArchive.Open(stream!), destinationPath, progressBarAction);
            case ".rar":
                return ProcessArchive<RarArchive, RarArchiveEntry, RarVolume>(isFile ? RarArchive.Open(file!) : RarArchive.Open(stream!), destinationPath, progressBarAction);
            case ".7z":
                return ProcessArchive<SevenZipArchive, SevenZipArchiveEntry, SevenZipVolume>(isFile ? SevenZipArchive.Open(file!) : SevenZipArchive.Open(stream!), destinationPath, progressBarAction);
            case ".tar":
                return ProcessArchive<TarArchive, TarArchiveEntry, TarVolume>(isFile ? TarArchive.Open(file!) : TarArchive.Open(stream!), destinationPath, progressBarAction);
            case ".gz":
                return ProcessArchive<GZipArchive, GZipArchiveEntry, GZipVolume>(isFile ? GZipArchive.Open(file!) : GZipArchive.Open(stream!), destinationPath, progressBarAction);
        }

        return Result<string, CommandError>.Error(new CommandError("Unknown archive type"));
    }

    public Result<string, CommandError> Extract(FileInfo file, string destinationPath, Action<double>? progressBarAction)
    {
        return ExtractSelector(file.Extension, destinationPath, file, true, null, progressBarAction);
    }

    public Result<string, CommandError> ExtractFromStream(Stream stream, string extension, string destinationPath, Action<double>? progressBarAction)
    {
        return ExtractSelector(extension, destinationPath, null, false, stream, progressBarAction);
    }

    public Result<string, CommandError> ExtractZipFromStream(Stream stream, string destinationPath, Action<double>? progressBarAction)
    {
        return ExtractFromStream(stream, ".zip", destinationPath, progressBarAction);
    }
}
