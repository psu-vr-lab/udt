using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UEScript.CLI.Commands;
using UEScript.CLI.Models;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services
{
    public interface IArchiveExtractor
    {
        public Result<string, CommandError> Extract(FileInfo file, string destinationPath);
        public Result<string, CommandError> ExtractFromStream(Stream stream, string extension, string destinationPath);
        public Result<string, CommandError> ExtractZipFromStream(Stream stream, string destinationPath);
    }
}
