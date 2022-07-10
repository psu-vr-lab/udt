using UnrealSetupper;
using System.Text.Json;

class Program
{
    public static string appDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.uepme\";
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

        if (!Directory.Exists(appDir))
        {
            Directory.CreateDirectory(appDir);
        }

        if (!File.Exists( appDir + @"UnrealSettuper.config.json"))
        {
            ConfigOutput();
        }

        if (!Directory.Exists(appDir + "Projects"))
        {
            Directory.CreateDirectory(appDir + "Projects");
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

            DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
            FileInfo[] files = projectsDir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (projectName == file.Name.Replace($".config.json", ""))
                {
                    USettuperProjectConfig? projectConfig = JsonSerializer.Deserialize<USettuperProjectConfig>(File.ReadAllText(appDir + @"Projects\" + $"{projectName}.config.json"));
                    if (projectConfig != null && projectConfig.ProjectDir != null)
                        System.Diagnostics.Process.Start("explorer.exe", $"{projectConfig.ProjectDir}");
                    Output.Succses("OK!");
                    return;
                }
            }
            Output.Error("There is no such project");

        }
        else if (userArgs.StartsWith("run", comparison))
        {
            string projectName = userArgs.Replace("run", "");

            DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
            FileInfo[] files = projectsDir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (projectName == file.Name.Replace($".config.json", ""))
                {
                    USettuperProjectConfig? projectConfig = JsonSerializer.Deserialize<USettuperProjectConfig>(File.ReadAllText(appDir + @"Projects\" + $"{projectName}.config.json"));
                    if (projectConfig != null && projectConfig.ProjectDir != null)
                    {
                        if(!File.Exists($"{projectConfig.ProjectDir}" + @"\Binaries\Win64\" + $"{projectConfig.Name}.exe"))
                        {
                            Output.Error("Compile project firts");
                            return;
                        }

                        System.Diagnostics.Process.Start($"{projectConfig.ProjectDir}" + @"\Binaries\Win64\" + $"{projectConfig.Name}.exe");
                    }
                    Output.Succses("OK!");
                    return;
                }
            }
            Output.Error("There is no such project");

        }
        else if (userArgs.StartsWith("Build", comparison))
        {
            BatRunOutput(userArgs, "Build");
        }
        else if (userArgs.StartsWith("Editor", comparison))
        {
            BatRunOutput(userArgs, "Editor");
        }
        else if (userArgs.StartsWith("Compile", comparison))
        {
            BatRunOutput(userArgs, "Compile");
        }
        else if (userArgs.StartsWith("Cook", comparison))
        {
            BatRunOutput(userArgs, "Cook");
        }
        else if (userArgs.StartsWith("Link", comparison))
        {
            USettuperConfig? config = JsonSerializer.Deserialize<USettuperConfig>(File.ReadAllText(appDir + "UnrealSettuper.config.json"));
            if (config == null || config.ProjectsDir == null || config.UnrealDir == null)
            {
                Output.Error("Config error!");
                return;
            }

            string projectPath = userArgs.Replace("link", "");
            DirectoryInfo projectsDir = new DirectoryInfo(projectPath);
            FileInfo[] files = projectsDir.GetFiles();
            bool wasFoundProjectBat = false;
            bool wasFoundUnrealProjectFile = false;

            string unrealProjectName = string.Empty;
            foreach (FileInfo file in files)
            {
                if ("Build.bat" == file.Name)
                {
                    wasFoundProjectBat = true;
                }

                if (file.Name.Contains(".uproject"))
                {
                    wasFoundUnrealProjectFile = true;
                    unrealProjectName = Path.GetFileNameWithoutExtension(projectPath + @"\" + $"{file.Name}");
                    Console.WriteLine(unrealProjectName);
                }
            }
            if (!wasFoundUnrealProjectFile)
            {
                Output.Error("No unreal files was found.");
                return;
            }

            USettuperProjectConfig projectConfig = new USettuperProjectConfig
            {
                Name = unrealProjectName,
                ProjectDir = projectPath
            };
            if (!wasFoundProjectBat)
            {
                Console.WriteLine("Start generating uepme build files");
                USettuper.GBB(Path.Combine(projectPath, "Build") + ".bat", unrealProjectName, config);
                USettuper.GCB(Path.Combine(projectPath, "Compile") + ".bat", unrealProjectName, config);
                USettuper.GCoB(Path.Combine(projectPath, "Cook") + ".bat", unrealProjectName, config);
                USettuper.GEB(Path.Combine(projectPath, "Editor") + ".bat", unrealProjectName, config);
            }

            string fileName = appDir + @"Projects\" + $"{unrealProjectName}.config.json";
            string configToJson = JsonSerializer.Serialize(projectConfig);
            File.WriteAllText(fileName, configToJson);
            Output.Succses("Ok!");

            
        }
        else if (userArgs.StartsWith("List", comparison))
        {
            Console.WriteLine("");
            DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
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

            DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
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
                File.Delete(appDir + @"Projects\" + $"{projectName}.config.json");
                Output.Succses("The project has been deleted");
            }
        }
        else
        {
            Output.Error("Wrong Argument!\n");
            Console.WriteLine("* config - to update the configuration\n* create - creates a new project\n* build - builds a project for the Editor\n* editor - launch editor\n* cook - cook content\n* compile - development build\n* run - start .exe of development build\n* list - output all projects\n* delete - delete the project configuration file\n* open - open the project folder in explorer\n* link - link unreal project with uepme");
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
        USettuperConfig? config = JsonSerializer.Deserialize<USettuperConfig>(File.ReadAllText(appDir + "UnrealSettuper.config.json"));
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
            Output.Error("Folder with that name already created, Use Link instead.");
            return;
        }

        USettuperProjectConfig projectConfig = new USettuperProjectConfig
        {
            Name = userArgs,
            ProjectDir = projectDir
        };

        
        string fileName = appDir + @"Projects\" + $"{userArgs}.config.json";
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
        USettuper.GCB(Path.Combine(projectDir, "Compile") + ".bat", userArgs, config);
        USettuper.GCoB(Path.Combine(projectDir, "Cook") + ".bat", userArgs, config);
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
        DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
        FileInfo[] files = projectsDir.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Name == projectName)
            {
                USettuperProjectConfig? projectConfig = JsonSerializer.Deserialize<USettuperProjectConfig>(File.ReadAllText(appDir + @"Projects\" + $"{userArgs.Replace($"{batFile.ToLower()}", "")}.config.json"));
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