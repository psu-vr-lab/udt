using Microsoft.Extensions.Logging;
using UEScript.Utils.Results;

namespace UEScript.Utils.Extensions;

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
            logger.LogError(result, errorPrefix);
        }
    }
    
    private static void LogError(this ILogger logger, Error error, string errorPrefix = "")
    {
        var message = errorPrefix +  error.GetMessage();
        logger.LogError(message);
    }
}