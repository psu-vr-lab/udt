using Microsoft.Extensions.Logging;
using UEScript.Utils.Extensions;
using UEScript.Utils.Results;
using UEScript.CLI.Common.Console;

namespace UEScript.CLI.Common;

public static class ConsoleLoggerExtensions
{
    public static ILoggingBuilder AddCustomFormatter(
        this ILoggingBuilder builder,
        Action<UEScriptConsoleFormatterOptions> configure)
    {
        builder.AddConsole(options => options.FormatterName = "ueco")
            .AddConsoleFormatter<UEScriptConsoleFormatter, UEScriptConsoleFormatterOptions>(configure);
        
        return builder;
    }

    public static void LogResult<TValue, TError>(this ILogger logger, Result<TValue, TError> result) where TError : Error
    {
        logger.LogTrace("--------------------------");
        logger.LogResult(result, "[OK] ", "[ERROR] ");
        System.Console.ResetColor(); // Reset color for Console
        // TODO: Refactor logger so i dont need this ^
    }
}