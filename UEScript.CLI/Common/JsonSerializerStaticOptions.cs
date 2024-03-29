using System.Text.Json;
using System.Text.Json.Serialization;

namespace UEScript.CLI.Common;

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

            _options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower));
        }

        return _options;
    }
}