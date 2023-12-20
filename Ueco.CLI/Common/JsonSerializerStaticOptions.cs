using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ueco.Common;

public static class JsonSerializerStaticOptions
{
    private static JsonSerializerOptions? _options;
    
    public static JsonSerializerOptions GetOptions()
    {
        if (_options is null)
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            _options.Converters.Add(new JsonStringEnumConverter());
        }

        return _options;
    }
}