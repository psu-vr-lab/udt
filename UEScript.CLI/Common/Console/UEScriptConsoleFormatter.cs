using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace UEScript.CLI.Common.Console;

public class UEScriptConsoleFormatter : ConsoleFormatter, IDisposable
{
    private readonly IDisposable? _optionsReloadToken;
    private UEScriptConsoleFormatterOptions _formatterOptions;
    
    public UEScriptConsoleFormatter(IOptionsMonitor<UEScriptConsoleFormatterOptions> optionsMonitor) : base("ueco")
    {
        _optionsReloadToken = optionsMonitor.OnChange(options => _formatterOptions = options);
        _formatterOptions = optionsMonitor.CurrentValue;
    }
    
    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        System.Console.ResetColor();
        
        var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
        if (message is null)
        {
            return;
        }
        
        if (_formatterOptions.ColorBehavior == LoggerColorBehavior.Enabled)
        {
            System.Console.ForegroundColor = logEntry.LogLevel switch
            {
                LogLevel.Error or LogLevel.Critical =>  _formatterOptions.ErrorColor,
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
            message = $"{DateTime.UtcNow:HH:mm:ss} {message}";
        }
        
        textWriter.WriteLine(message);
    }
    
    public void Dispose()
    {
        _optionsReloadToken?.Dispose();
    }
}