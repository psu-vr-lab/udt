using System.Text.Json;
using System.Text.Json.Serialization;

namespace UEScript.CLI.Models;

/// <summary>
/// Contains and manages settings for UEScript.
/// </summary>
public sealed class Settings
{
    /// <summary>
    /// Extensions of files that can be compressed to zip with lfs files.
    /// If not set, all files will be compressed.
    /// </summary>
    public List<string> TargetExtensions { get; set; } = [];
    /// <summary>
    /// Default path for install Unreal Engine.
    /// if not set, you need pass it manually form CLI.
    /// Use to set default path for install.
    /// </summary>
    public string? DefaultInstallPathFromSetting { get; set; }
    /// <summary>
    /// Default path for zips with Unreal Engine LFS project files.
    /// if not set, you need pass it manually form CLI.
    /// Use to set default path for zips.
    /// </summary>
    public string? DefaultZipFolderFromSetting { get; set; }
    /// <summary>
    /// Max file size that can be compressed to zip with lfs files.
    /// Default is -1 (unlimited).
    /// File size in bytes.
    /// </summary>
    public int? MaxLfsFileSize { get; private set; }
    /// <summary>
    /// Min file size that can be compressed to zip with lfs files.
    /// Default is -1 (unlimited).
    /// File size in bytes.
    /// </summary>
    public int? MinLfsFileSize { get; private set; }
    /// <summary>
    /// Flag to set default settings if true or setting not valid.
    /// </summary>
    public bool IsDefault { get; set; } = true;
    /// <summary>
    /// App setting path.
    /// Can not be changed.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    private readonly string _settingsPath = AppContext.BaseDirectory + "Settings.json";
    /// <summary>
    /// Default path for zips with Unreal Engine LFS project files.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public FileInfo? DefaultZipFolder { get; private set; }
    /// <summary>
    /// Default path for install Unreal Engine.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public FileInfo? DefaultInstallPath { get; private set; }
    

    /// <summary>
    /// Load and set settings from Settings.json.
    /// If settings not valid, set default settings.
    /// </summary>
    public void Load()
    {
        var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(_settingsPath));

        if (settings == null)
        {
            DefaultSettings();
            return;
        }
        
        if (settings.IsDefault)
        {
            DefaultSettings();
            return;
        }

        if (!settings.ValidateSettings())
        {
            return;
        }
        
        SetExtensionsFromModel(settings);
        SetFileSizes(settings);
        DefaultInstallPathFromSetting = settings.DefaultInstallPathFromSetting;
        DefaultZipFolderFromSetting = settings.DefaultZipFolderFromSetting;
        SetsDefaultInstallPath();
        SetsDefaultZipFolder();
    }
    
    /// <summary>
    /// Set default path for install.
    /// </summary>
    /// <param name="path"></param>
    public void SetsDefaultInstallPath(string path)
    {
        DefaultInstallPath = new FileInfo(path);
    }
    
    /// <summary>
    /// Set default path for zips.
    /// </summary>
    /// <param name="path"></param>
    public void SetsDefaultZipFolder(string path)
    {
        DefaultZipFolder = new FileInfo(path);
    }

    /// <summary>
    /// Add extension to settings if it valid.
    /// </summary>
    public void AddExtension(string extension)
    {
        if (!TargetExtensions.Contains(extension))
        {
            return;
        }

        if (!ValidateExtension(extension))
        {
            return;
        }
        
        TargetExtensions.Add(extension);
    }

    /// <summary>
    /// Remove extension from settings.
    /// </summary>
    public void RemoveExtension(string extension)
    {
        TargetExtensions.Remove(extension);
    }
    
    /// <summary>
    /// Clear extensions from settings.
    /// </summary>
    public void ClearExtensions()
    {
        TargetExtensions.Clear();
    }

    /// <summary>
    /// Save settings to Settings.json
    /// </summary>
    public void Save()
    {
        var settingsJson = JsonSerializer.Serialize(this);
        
        if (!ValidateSettings())
        {
            return;
        }
        
        File.CreateText(_settingsPath).Write(settingsJson);
    }
    
    /// <summary>
    /// Map settings from settings object.
    /// Use this to manually set settings.
    /// if not some property not valid, it will be not set.
    /// </summary>
    /// <param name="settings"></param>
    public void MapFrom(Settings settings)
    {
        SetExtensionsFromModel(settings);
        SetFileSizes(settings);
    }
    
    /// <summary>
    /// use to manually set lfs file sizes.
    /// if not valid, use default.
    /// </summary>
    /// <param name="minLfsFileSize"></param>
    /// <param name="maxLfsFileSize"></param>
    public void SetFileSizes(int minLfsFileSize, int maxLfsFileSize)
    {
        MinLfsFileSize = minLfsFileSize;
        MaxLfsFileSize = maxLfsFileSize;
        ValidateFileSizes();
    }

    /// <summary>
    /// Sets default settings.
    /// </summary>
    public void DefaultSettings()
    {
        TargetExtensions = [];
        DefaultZipFolderFromSetting = null;
        DefaultInstallPathFromSetting = null;
        MaxLfsFileSize = -1;
        MinLfsFileSize = -1;
        IsDefault = true;
    }
    
    /// <summary>
    /// Set default file sizes.
    /// </summary>
    public void SetDefaultFileSizes()
    {
        MinLfsFileSize = -1;
        MaxLfsFileSize = -1;
    }

    /// <summary>
    /// Validate settings.
    /// If not something is not valid, sets default settings.
    /// </summary>
    /// <returns>bool</returns>
    private bool ValidateSettings()
    {
        var fileSizesValid = ValidateFileSizes();
        var extensionsValid = ValidateExtensions();
        
        return fileSizesValid && extensionsValid;
    }
    
    /// <summary>
    /// Validate default install path.
    /// </summary>
    private void SetsDefaultInstallPath()
    {
        if (DefaultInstallPathFromSetting == null)
        {
            return;
        }
        
        DefaultInstallPath = new FileInfo(DefaultInstallPathFromSetting);
    }
    
    /// <summary>
    /// Validate default zip folder.
    /// </summary>
    private void SetsDefaultZipFolder()
    {
        if (DefaultZipFolderFromSetting == null)
        {
            return;
        }
        
        DefaultZipFolder = new FileInfo(DefaultZipFolderFromSetting);
    }
    
    /// <summary>
    /// Validate extensions from settings.
    /// </summary>
    private bool ValidateExtensions()
    {
        return TargetExtensions.All(ValidateExtension);
    }
    
    /// <summary>
    /// Validate file sizes. 
    /// </summary>
    /// <returns>bool</returns>
    private bool ValidateFileSizes()
    {
        if (MaxLfsFileSize < -1)
        {
            return false;
        }
        
        if (MinLfsFileSize < -1)
        {
            return false;
        }

        return MinLfsFileSize > MaxLfsFileSize;
    }
    
    /// <summary>
    /// set file sizes from settings.
    /// </summary>
    /// <param name="settings"></param>
    private void SetFileSizes(Settings? settings)
    {
        MinLfsFileSize = settings?.MinLfsFileSize;
        MaxLfsFileSize = settings?.MaxLfsFileSize;
    }
    
    /// <summary>
    /// Set target extensions from settings.
    /// </summary>
    /// <param name="settings"></param>
    private void SetExtensionsFromModel(Settings settings)
    {
        var extensions = new HashSet<string>();

        var result = settings.TargetExtensions.All(extensions.Add);

        if (!result)
        {
            return;
        }
        
        TargetExtensions = extensions.ToList();
    }
    
    /// <summary>
    /// Validate extension.
    /// </summary>
    private bool ValidateExtension(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension) || !extension.StartsWith('.'))
        {
            return false;    
        }

        return extension[^1..].All(char.IsLetterOrDigit);
    }
}