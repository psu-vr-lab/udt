using System.Reflection;
using UEScript.CLI.Configurations;

[assembly: AssemblyVersion("3.1.*")]

namespace UEScript.CLI;

public static class  Program
{
    public static Task Main(string[] args)
    {
        var cliConfiguration = ProgramConfiguration
            .BuildCommandLine()
            .Configure(args);
        
        return cliConfiguration.InvokeAsync(args);
    }
}