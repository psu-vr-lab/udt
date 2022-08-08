using UnrealSetupper;
using System.Text.Json;
using CommandLine;
using System.Reflection;
using Console = Colorful.Console;
using Colorful;
using CommandLine.Text;
using System.Net;

class Program
{
    public static string appDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.uepme\";
    private static void Main(string[] args)
    {
        // Check if Uepme allready configured, if not then create dirs and config
        if (!Directory.Exists(appDir))
        {
            Directory.CreateDirectory(appDir);
        }
        if (!Directory.Exists(appDir + "Projects"))
        {
            Directory.CreateDirectory(appDir + "Projects");
        }if (!Directory.Exists(appDir + "font"))
        {
            Directory.CreateDirectory(appDir + "font");
            // download font
            using (var client = new WebClient())
            {
                client.DownloadFile("http://www.figlet.org/fonts/larry3d.flf", $"{appDir}/font/larry3d.flf");
            }
        }

        if (!File.Exists( appDir + @"UnrealSettuper.config.json"))
        {
            new Config().Run();
            return;
        }

        // disable default help
        // var parser = new Parser(with => with.HelpWriter = null);
        var parserResult = new Parser(c => c.HelpWriter = null).ParseArguments<
            Config, New, Open, RunVerb,
            Build, Editor, Compile,
            Cook, Link, List, Delete
            >(args);
        parserResult.MapResult(
            (BaseVerb o) =>
            {
                o.Run();
                return 1;
            },
            e => DisplayHelp(parserResult));
    }

    static int DisplayHelp(ParserResult<object> parserResult)
    {
        FigletFont font = FigletFont.Load($"{appDir}/font/larry3d.flf");
        Figlet figlet = new Figlet(font);
        Console.WriteLine(figlet.ToAscii("uepme"));
        var versionString = Assembly.GetEntryAssembly()?
                                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                .InformationalVersion
                                .ToString();
        Console.WriteLine(HelpText.AutoBuild(parserResult, h => {
            h.AdditionalNewLineAfterOption = false;
            h.Heading = $"uepme v{versionString} - unreal engine project manager";
            h.Copyright = "apache-2.0 license, jejikeh@gmail.com";
            return h;
        }));
        return 1;
    }
}