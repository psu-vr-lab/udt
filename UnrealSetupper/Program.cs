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
            string unrealDir = config.UnrealDir;
            string projectDir = Path.Combine(config.ProjectsDir, userArgs);
            string sourceDir = Path.Combine(projectDir, "Source");
            string coreDir = Path.Combine(sourceDir, $"{userArgs}Core");
            string privateDir = Path.Combine(coreDir,"Private");
            string publicDir = Path.Combine(coreDir, "Public");

            Console.WriteLine("Creating Directories");
            Directory.CreateDirectory(projectDir);
            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(coreDir);
            Directory.CreateDirectory(privateDir);
            Directory.CreateDirectory(publicDir);

            Console.WriteLine("Generating unreal project file");
            USettuper.UUPROJ(Path.Combine(projectDir, userArgs) + ".uproject",userArgs);
            Console.WriteLine("Generating Unreal Module Target for Standalone mode");
            USettuper.UMT(Path.Combine(sourceDir,userArgs) + ".Target.cs", userArgs,"Game");
            Console.WriteLine("Generating Unreal Module Target for Editor mode");
            USettuper.UMT(Path.Combine(sourceDir, userArgs) + "Editor.Target.cs", userArgs, "Editor");
            Console.WriteLine("Creating Unreal Module Build");
            USettuper.UMB(Path.Combine(coreDir, userArgs) + "Core.Build.cs", userArgs);
            Console.WriteLine("Creating Unreal Module Header");
            USettuper.UMH(Path.Combine(publicDir, userArgs) + "Core.h", userArgs);
            Console.WriteLine("Generating unreal module cpp primary");
            USettuper.UMC(Path.Combine(privateDir, userArgs) + "Core.cpp", userArgs);
            Console.WriteLine("Unreal Log Setup");
            USettuper.ULH(Path.Combine(privateDir, "Log") + ".h", userArgs);
            USettuper.ULC(Path.Combine(privateDir, "Log") + ".cpp", userArgs);
            Console.WriteLine("Generating .bat files");
            USettuper.GBB(Path.Combine(projectDir, "Build") + ".bat", userArgs,config);
            USettuper.GEB(Path.Combine(projectDir, "Editor") + ".bat", userArgs, config);

            Console.WriteLine($"Start Build {userArgs} unreal project");
            ExecuteCommand($"{Path.Combine(projectDir, "Build") + ".bat"}");
            Console.WriteLine($"Exec Editor");
            ExecuteCommand($"{Path.Combine(projectDir, "Editor") + ".bat"}");
            System.Diagnostics.Process.Start($"{Path.Combine(projectDir, "Editor") + ".bat"}");
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

    private static void ExecuteCommand(string command)
    {
        System.Diagnostics.Process process;
        System.Diagnostics.ProcessStartInfo processStartInfo;
        processStartInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + command);
        processStartInfo.CreateNoWindow = true;
        process = System.Diagnostics.Process.Start(command);
        process.WaitForExit();

        int exitCode = process.ExitCode;
        process.Close();
        Console.WriteLine($"ExitCode {exitCode}");
    }
}