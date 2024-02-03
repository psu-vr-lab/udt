using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace UEScript.Logging;

public class UEScriptLogger(string name, Func<UEScriptLoggerConfiguration> getConfiguration) : ILogger
{
    private readonly Stopwatch _timeSinceLastLog = new Stopwatch();
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var config = getConfiguration();

        var initialColor = Console.ForegroundColor;
        
        Console.ForegroundColor = config.LogLevelColors[logLevel];
        Console.WriteLine($"[{_timeSinceLastLog.Elapsed:mm\\:ss\\.ff}] {formatter(state, exception)}");
        
        Console.ForegroundColor = initialColor;
        
        _timeSinceLastLog.Restart();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return getConfiguration().LogLevelColors.ContainsKey(logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default;
    }
}