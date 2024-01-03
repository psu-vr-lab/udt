using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UEScript.Logging;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("UEScript")]
public class UEScriptLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeConfiguration;
    private UEScriptLoggerConfiguration _currentConfiguration;
    private readonly ConcurrentDictionary<string, UEScriptLogger> _loggers 
        = new ConcurrentDictionary<string, UEScriptLogger>(StringComparer.OrdinalIgnoreCase);

    public UEScriptLoggerProvider(IOptionsMonitor<UEScriptLoggerConfiguration> configuration)
    {
        _currentConfiguration = configuration.CurrentValue;
        _onChangeConfiguration = configuration.OnChange(config => _currentConfiguration = config);
    }
    
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(
            categoryName, 
            name => new UEScriptLogger(name, () => _currentConfiguration));
    }

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeConfiguration?.Dispose();
    }
}
