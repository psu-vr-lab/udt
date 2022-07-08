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

        StringComparison comparison = StringComparison.InvariantCultureIgnoreCase;

        if (userArgs.StartsWith("config", comparison))
        {
            ConfigOutput();
        }
        else if (userArgs.StartsWith("create", comparison))
        {
            string projectName = userArgs.Replace("create", "");
            CreateOutput(projectName);
        }
        else if (userArgs.StartsWith("open", comparison))
        {
            string projectName = userArgs.Replace("open", "");

            DirectoryInfo projectsDir = new DirectoryInfo(@"Projects");
            FileInfo[] files = projectsDir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (projectName == file.Name.Replace($".config.json", ""))
                {
                    USettuperProjectConfig? projectConfig = JsonSerializer.Deserialize<USettuperProjectConfig>(File.ReadAllText(@"Projects\" + $"{projectName}.config.json"));
                    if (projectConfig != null && projectConfig.ProjectDir != null)
                        System.Diagnostics.Process.Start("explorer.exe", $"{projectConfig.ProjectDir}");
                    Output.Succses("OK!");
                    return;
                }
            }
            Output.Error("There is no such project");

        }
        else if (userArgs.StartsWith("build", comparison))
        {
            BatRunOutput(userArgs, "Build");
        }
        else if (userArgs.StartsWith("editor", comparison))
        {
            BatRunOutput(userArgs, "Editor");
        }
        else if (userArgs.StartsWith("list", comparison))
        {
            Console.WriteLine("");
            DirectoryInfo projectsDir = new DirectoryInfo(@"Projects");
            FileInfo[] files = projectsDir.GetFiles();
            int i = 0;
            foreach (FileInfo file in files)
            {
                i++;
                Console.WriteLine($"{i,3}. " + $"{file.Name.Replace($".config.json", ""),10}" + $"\t{file.LastWriteTime,25}");
            }

            Console.WriteLine();
        }
        else if (userArgs.StartsWith("delete", comparison))
        {
            string projectName = userArgs.Replace("delete", "");

            DirectoryInfo projectsDir = new DirectoryInfo(@"Projects");
            FileInfo[] files = projectsDir.GetFiles();
            bool wasFound = false;
            foreach (FileInfo file in files)
            {
                if(projectName == file.Name.Replace($".config.json", ""))
                {
                    wasFound = true;
                }
            }
            if (!wasFound)
            {
                Output.Error("There is no such project");
                return;
            }

            Console.WriteLine("All project files will remain in place.");
            Output.Error("Delete the configuration file?");

            Console.Write("\nYes or No : ");

            string? userChoose = Console.ReadLine();
            if(userChoose == null || !userChoose.ToLower().StartsWith("y") && userChoose.ToLower().StartsWith("n"))
            {
                return;
            }
            if (userChoose.ToLower().StartsWith("y"))
            {
                File.Delete(@"Projects\" + $"{projectName}.config.json");
                Output.Succses("The project has been deleted");
            }
        }
        else
        {
            Output.Error("Wrong Argument!\n");
            Console.WriteLine("* config - to update the configuration\n* create - creates a new project\n* build - builds a project for the Editor\n* editor - launch editor\n* list - output all projects\n* delete - delete the project configuration file\n* open - open the project folder in explorer");
        }
    }

    private static void ConfigOutput()
    {
        Console.WriteLine("Generating config...");
        Console.WriteLine("All paths without quotes");
        Console.Write("Unreal path: ");
        string? unrealDir = Console.ReadLine();
        Console.Write("Projects directory path: ");
        string? projectsDir = Console.ReadLine();

        if(unrealDir != null && projectsDir != null && !unrealDir.Contains(@"""") && !projectsDir.Contains(@""""))
        {
            USettuper.Config(unrealDir, projectsDir);
            Output.Succses("Config updated!");
        }else
        {
            Output.Error("Wrong path!");
        }
    }

    private static void CreateOutput(string userArgs)
    {
        USettuperConfig? config = JsonSerializer.Deserialize<USettuperConfig>(File.ReadAllText("UnrealSettuper.config.json"));
        if (config == null || config.ProjectsDir == null || config.UnrealDir == null)
        {
            Output.Error("Config error!");
            return;
        }
        string projectDir = Path.Combine(config.ProjectsDir, userArgs);
        string sourceDir = Path.Combine(projectDir, "Source");
        string coreDir = Path.Combine(sourceDir, $"{userArgs}Core");
        string privateDir = Path.Combine(coreDir, "Private");
        string publicDir = Path.Combine(coreDir, "Public");

        if (Directory.Exists(projectDir))
        {
            Output.Error("Folder with that name already created!");
            return;
        }

        USettuperProjectConfig projectConfig = new USettuperProjectConfig
        {
            Name = userArgs,
            ProjectDir = projectDir
        };

        if (!Directory.Exists("Projects"))
        {
            Directory.CreateDirectory("Projects");
        }
        string fileName = @"Projects\" + $"{userArgs}.config.json";
        string configToJson = JsonSerializer.Serialize(projectConfig);
        File.WriteAllText(fileName, configToJson);

        Console.WriteLine("Creating Directories");
        Directory.CreateDirectory(projectDir);
        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(coreDir);
        Directory.CreateDirectory(privateDir);
        Directory.CreateDirectory(publicDir);

        USettuper.UUPROJ(Path.Combine(projectDir, userArgs) + ".uproject", userArgs);
        USettuper.UMT(Path.Combine(sourceDir, userArgs) + ".Target.cs", userArgs, "Game");
        USettuper.UMT(Path.Combine(sourceDir, userArgs) + "Editor.Target.cs", userArgs, "Editor");
        USettuper.UMB(Path.Combine(coreDir, userArgs) + "Core.Build.cs", userArgs);
        USettuper.UMH(Path.Combine(publicDir, userArgs) + "Core.h", userArgs);
        USettuper.UMC(Path.Combine(privateDir, userArgs) + "Core.cpp", userArgs);
        USettuper.ULH(Path.Combine(privateDir, "Log") + ".h", userArgs);
        USettuper.ULC(Path.Combine(privateDir, "Log") + ".cpp", userArgs);
        USettuper.GBB(Path.Combine(projectDir, "Build") + ".bat", userArgs, config);
        USettuper.GEB(Path.Combine(projectDir, "Editor") + ".bat", userArgs, config);

        Console.WriteLine($"\nThe first build of the {userArgs} project");
        ExecuteCommand($"{Path.Combine(projectDir, "Build") + ".bat"}");
        Console.WriteLine($"Exec Editor");
        System.Diagnostics.Process.Start($"{Path.Combine(projectDir, "Editor") + ".bat"}");
    }

    private static void BatRunOutput(string userArgs,string batFile)
    {
        Console.WriteLine($"Launching the {batFile} of Project {userArgs.Replace($"{batFile.ToLower()}", "")}");
        string projectName = userArgs.Replace($"{batFile.ToLower()}", "") + ".config.json";
        DirectoryInfo projectsDir = new DirectoryInfo(@"Projects");
        FileInfo[] files = projectsDir.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Name == projectName)
            {
                USettuperProjectConfig? projectConfig = JsonSerializer.Deserialize<USettuperProjectConfig>(File.ReadAllText(@"Projects\" + $"{userArgs.Replace($"{batFile.ToLower()}", "")}.config.json"));
                if (projectConfig != null && projectConfig.ProjectDir != null)
                    ExecuteCommand($"{Path.Combine(projectConfig.ProjectDir, batFile) + ".bat"}");
                Output.Succses("OK!");
                return;
            }
        }
        Output.Error("There is no such project");
    }

    private static void ExecuteCommand(string command)
    {
        System.Diagnostics.Process process;
        System.Diagnostics.ProcessStartInfo processStartInfo;
        processStartInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + command);
        processStartInfo.CreateNoWindow = false;
        process = System.Diagnostics.Process.Start(command);
        if (command.Contains("Editor"))
        {
            return;
        }
        process.WaitForExit();

        int exitCode = process.ExitCode;
        process.Close();
        Console.WriteLine($"ExitCode {exitCode}");
    }
}