using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Ueco.Common.Console;

public class UecoConsoleFormatter : ConsoleFormatter, IDisposable
{
    private readonly IDisposable? _optionsReloadToken;
    private UecoConsoleFormatterOptions _formatterOptions;
    private readonly ConsoleColor _defaultColor = System.Console.ForegroundColor;

    
    public UecoConsoleFormatter(IOptionsMonitor<UecoConsoleFormatterOptions> optionsMonitor) : base("ueco")
    {
        _optionsReloadToken = optionsMonitor.OnChange(options => _formatterOptions = options);
        _formatterOptions = optionsMonitor.CurrentValue;
    }
    
    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        System.Console.ForegroundColor = _defaultColor;
        
        var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
        if (message is null)
        {
            return;
        }
        
        if (_formatterOptions.ColorBehavior == LoggerColorBehavior.Enabled)
        {
            System.Console.ForegroundColor = logEntry.LogLevel switch
            {
                LogLevel.Error or LogLevel.Critical => _formatterOptions.ErrorColor,
                LogLevel.Warning => _formatterOptions.WarningColor,
                LogLevel.Trace => _formatterOptions.TraceColor,
                LogLevel.Information => _formatterOptions.InformationColor,
                LogLevel.Debug => _formatterOptions.DebugColor,
                LogLevel.None => _formatterOptions.NoneColor,
                _ => System.Console.ForegroundColor
            };
        }
        
        if (_formatterOptions.UseUtcTimestamp)
        {
            message = $"{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss.fffffffZ} {message}";
        }

        textWriter.WriteLine(message);
    }
    
    public void Dispose()
    {
        _optionsReloadToken?.Dispose();
    }
}