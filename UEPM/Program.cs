using System.CommandLine;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ueco.Commands;
using Ueco.Configuration;

namespace Ueco;

public static class  Program
{
    private static async Task<int> Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddJsonCommandSettings();

        var configuration = serviceCollection.BuildServiceProvider().GetService<IConfiguration>();
        Debug.Assert(configuration is not null, "configuration is null");
        
        var rootCommand = ConfigureRootCommand.AddRootCommand(configuration);
        return await rootCommand.InvokeAsync(args);
    }
}