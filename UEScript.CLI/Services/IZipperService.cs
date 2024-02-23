namespace UEScript.CLI.Services;

public interface IZipperService
{
    public IEnumerable<string> TargetExtensions { get; set; }
    public void Zip(string sourcePath, string targetPath);
    public void Unzip(string sourcePath, string targetPath);
    public void CollectExtensions(string sourcePath);
}