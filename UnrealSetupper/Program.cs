using UnrealSetupper;
using System.Text.Json;

class Program
{
    private static void Main(string[] args)
    {
        string userArgs = string.Empty;
        if(args.Length > 0)
        {
            foreach(string arg in args)
            {
                userArgs += arg;
            }
        }

        if (!File.Exists("UnrealSettuper.config.json"))
        {
            ConfigOutput();
        }

        if (userArgs == "config")
        {
            ConfigOutput();
        }
        else
        {
            string configName = "UnrealSettuper.config.json";
            USettuperConfig? config = JsonSerializer.Deserialize<USettuperConfig>(File.ReadAllText(configName));
            if(config == null || config.ProjectsDir == null || config.UnrealDir == null)
            {
                return;
            }
            string projectDir = Path.Combine(config.ProjectsDir, userArgs);
            string sourceDir = Path.Combine(projectDir, "Source");
            string coreDir = Path.Combine(sourceDir, $"{userArgs}Core");
            string privateDir = Path.Combine(coreDir,"Private");
            string publicDir = Path.Combine(coreDir, "Public");

            Directory.CreateDirectory(projectDir);
            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(coreDir);
            Directory.CreateDirectory(privateDir);
            Directory.CreateDirectory(publicDir);


            USettuper.UUPROJ(Path.Combine(projectDir, userArgs) + ".uproject",userArgs);
            USettuper.UMT(Path.Combine(sourceDir,userArgs) + ".Target.cs", userArgs,"Game");
            USettuper.UMT(Path.Combine(sourceDir, userArgs) + "Editor.Target.cs", userArgs, "Editor");
            USettuper.UMB(Path.Combine(coreDir, userArgs) + "Core.Build.cs", userArgs);
            USettuper.UMH(Path.Combine(publicDir, userArgs) + "Core.h", userArgs);
        }
    }

    private static void ConfigOutput()
    {
        Console.Write("Unreal path: ");
        string? unrealDir = Console.ReadLine();
        Console.Write("Projects directory path: ");
        string? projectsDir = Console.ReadLine();

        USettuper.Config(unrealDir, projectsDir);
    }
}