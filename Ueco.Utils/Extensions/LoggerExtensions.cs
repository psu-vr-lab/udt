using Microsoft.Extensions.Logging;
using Ueco.Utils.Results;

namespace Ueco.Utils.Extensions;

public static class LoggerExtensions
{
    public static void LogResult<TValue, TError>(this ILogger logger, Result<TValue, TError> result, string okPrefix = "", string errorPrefix = "") where TError : Error
    {
        if (result.IsSuccess)
        {
            logger.LogInformation(okPrefix + result.GetValue());
        }
        else
        {
            logger.LogError(result.GetErrors(), errorPrefix);
        }
    }
    
    private static void LogError(this ILogger logger, Error[] errors, string errorPrefix = "")
    {
        foreach (var error in errors)
        {
            var message = errorPrefix +  error.GetMessage();
            logger.LogError(message);
        }
    }
}