using System.CommandLine.Binding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ueco.Commands.Binders;

public class AppConfigurationBinder : BinderBase<IConfiguration>
{
    protected override IConfiguration GetBoundValue(BindingContext bindingContext)
    {
        return GetConfiguration(bindingContext);
    }

    private static IConfiguration GetConfiguration(IServiceProvider bindingContext)
    {
        return bindingContext.GetService<IConfiguration>() ?? throw new InvalidOperationException();
    }
}