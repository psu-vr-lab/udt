using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace UEScript.Logging;

public static class UEScriptLoggerExtensions
{
    public static ILoggingBuilder AddUEScriptLogger(
        this ILoggingBuilder builder, 
        Action<UEScriptLoggerConfiguration> configure)
    {
        builder.AddConfiguration();
        
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, UEScriptLoggerProvider>());
        
        LoggerProviderOptions.RegisterProviderOptions
            <UEScriptLoggerConfiguration, UEScriptLoggerProvider>(builder.Services);
        
        builder.Services.Configure(configure);

        return builder;
    }
}