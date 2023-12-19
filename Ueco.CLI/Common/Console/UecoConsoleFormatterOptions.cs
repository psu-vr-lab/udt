using Microsoft.Extensions.Logging.Console;

namespace Ueco.Common.Console;

public class UecoConsoleFormatterOptions : ConsoleFormatterOptions
{
    public LoggerColorBehavior ColorBehavior { get; set; }
    public ConsoleColor WarningColor { get; set; }
    public ConsoleColor TraceColor { get; set; }
    public ConsoleColor ErrorColor { get; set; }
    public ConsoleColor DebugColor { get; set; }
    public ConsoleColor InformationColor { get; set; }
    public ConsoleColor NoneColor { get; set; }
}