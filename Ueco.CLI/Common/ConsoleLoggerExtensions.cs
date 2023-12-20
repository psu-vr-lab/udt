using Microsoft.Extensions.Logging;
using Ueco.Common.Console;
using Ueco.Utils.Extensions;
using Ueco.Utils.Results;

namespace Ueco.Common;

public static class ConsoleLoggerExtensions
{
    public static ILoggingBuilder AddCustomFormatter(
        this ILoggingBuilder builder,
        Action<UecoConsoleFormatterOptions> configure)
    {
        builder.AddConsole(options => options.FormatterName = "ueco")
            .AddConsoleFormatter<UecoConsoleFormatter, UecoConsoleFormatterOptions>(configure);
        
        return builder;
    }

    public static void LogResult<TValue, TError>(this ILogger logger, Result<TValue, TError> result) where TError : Error
    {
        logger.LogTrace("--------------------------");
        logger.LogResult(result, "[OK] ", "[ERROR] ");
    }
}