using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ueco.Configuration;

public static class SetupJsonConfigurationBuilder
{
    public static IServiceCollection AddJsonCommandSettings(this IServiceCollection services)
    {
        var builder = new ConfigurationBuilder();
        return services.AddSingleton<IConfiguration>(
            builder
                .AddJsonCommandSettings()
                .Build());
    }

    private static IConfigurationBuilder AddJsonCommandSettings(this IConfigurationBuilder builder)
    {
        var commandsFolder = Path.Combine(
            Path.GetDirectoryName(typeof(SetupJsonConfigurationBuilder).Assembly.Location)
            ?? throw new Exception("Assembly not found"),
            "Commands");

        if (commandsFolder is null)
        {
            throw new Exception("Command folder not found. They should be in the same folder as the executable");
        }

        foreach (var commandDirectory in Directory.GetDirectories(commandsFolder))
        {
            foreach (var commandFile in Directory.GetFiles(commandDirectory))
            {
                if (commandFile.StartsWith("command.") && !commandFile.EndsWith(".json"))
                {
                    continue;
                }

                Console.WriteLine(commandFile);
                builder.AddJsonFile(commandFile);
            }
        }

        return builder;
    }
}