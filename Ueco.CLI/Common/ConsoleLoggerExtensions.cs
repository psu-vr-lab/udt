using Microsoft.Extensions.Logging;
using Ueco.Common.Console;

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
}