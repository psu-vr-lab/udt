using System.Text.Json;

namespace UEScript.CLI.Services.Impl;

public class ZipperService : IZipperService
{
    public IEnumerable<string> TargetExtensions { get; set; }
    public void Zip(string sourcePath, string targetPath)
    {
        throw new NotImplementedException();
    }

    public void Unzip(string sourcePath, string targetPath)
    {
        
    }

    public void CollectExtensions(string sourcePath)
    {
        
    }
}