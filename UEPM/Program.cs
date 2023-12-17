using System.CommandLine;
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
        serviceCollection.AddCommands();
        
        return await serviceCollection.BuildServiceProvider().GetService<RootCommand>().InvokeAsync(args);
    }
}