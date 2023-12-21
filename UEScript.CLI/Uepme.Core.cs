// using CommandLine;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;
// using UnrealSetupper;
//
// /// <summary>
// /// Abstract class for all verbs
// /// </summary>
// internal abstract class BaseVerb
// {
//     protected static string appDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.uepme\";
//
//     internal abstract void Run();
// }
//
// [Verb("config", HelpText = "Update the configuration.")]
// internal class Config : BaseVerb
// {
//     internal override void Run()
//     {
//         Console.WriteLine("Generating config...");
//         Console.WriteLine("All paths without quotes");
//         Console.Write("Unreal path: ");
//         string? unrealDir = Console.ReadLine();
//         Console.Write("Projects directory path: ");
//         string? projectsDir = Console.ReadLine();
//
//         if (unrealDir != null && projectsDir != null && !unrealDir.Contains(@"""") && !projectsDir.Contains(@""""))
//         {
//             USettuper.Config(unrealDir, projectsDir);
//             Output.Succses("Config updated!");
//         }
//         else
//         {
//             Output.Error("Wrong path!");
//         }
//     }
// }
//
//
// [Verb("new", HelpText = "Creates an Unreal c++ project and launches the editor.")]
// internal class New : BaseVerb
// {
//     [Option('n', "name", Required = true, HelpText = "Project name.")]
//     public string? Name { get; set; }
//
//     [Option('c', "config", Required = false, HelpText = "Use the configuration file.")]
//     public bool UseConfig { get; set; }
//
//     [Option('b', "build", Required = false, HelpText = "Building a project after its creation.")]
//     public bool AutoBuild { get; set; }
//
//     internal override void Run()
//     {
//         USettuperConfig? config = JsonSerializer.Deserialize<USettuperConfig>(File.ReadAllText(appDir + "UnrealSettuper.config.json"));
//         if (config == null || config.ProjectsDir == null || config.UnrealDir == null)
//         {
//             Output.Error("Config error!");
//             return;
//         }
//         string projectDir = Path.Combine(UseConfig ? config.ProjectsDir : "", Name!);
//         string sourceDir = Path.Combine(projectDir, "Source");
//         string coreDir = Path.Combine(sourceDir, $"{Name}Core");
//         string privateDir = Path.Combine(coreDir, "Private");
//         string publicDir = Path.Combine(coreDir, "Public");
//         string configDir = Path.Combine(projectDir, "Config");
//
//         if (Directory.Exists(projectDir))
//         {
//             Output.Error("Folder with that name already created, Use Link instead.");
//             return;
//         }
//
//         USettuperProjectConfig projectConfig = new USettuperProjectConfig
//         {
//             Name = Name,
//             ProjectDir = UseConfig ? projectDir : Path.GetFullPath(projectDir),
//         };
//
//
//         string fileName = appDir + @"Projects\" + $"{Name}.config.json";
//         string configToJson = JsonSerializer.Serialize(projectConfig);
//         File.WriteAllText(fileName, configToJson);
//
//         Console.WriteLine("Creating Directories");
//         Directory.CreateDirectory(projectDir);
//         Directory.CreateDirectory(sourceDir);
//         Directory.CreateDirectory(coreDir);
//         Directory.CreateDirectory(privateDir);
//         Directory.CreateDirectory(publicDir);
//         Directory.CreateDirectory(configDir);
//
//         USettuper.UUPROJ(Path.Combine(projectDir, Name!) + ".uproject", Name!);
//
//         USettuper.UMT(Path.Combine(sourceDir, Name!) + ".Target.cs", Name!, "Game");
//         USettuper.UMT(Path.Combine(sourceDir, Name!) + "Editor.Target.cs", Name!, "Editor");
//         USettuper.UMB(Path.Combine(coreDir, Name!) + "Core.Build.cs", Name!);
//
//         //USettuper.UGBH(Path.Combine(coreDir, userArgs) + "GameModeBase.h", userArgs);
//         //USettuper.UGBC(Path.Combine(coreDir, userArgs) + "GameModeBase.cpp", userArgs);
//
//         USettuper.UMH(Path.Combine(publicDir, Name!) + "Core.h", Name!);
//         USettuper.UTH(Path.Combine(publicDir, "ActorTest") + ".h", Name!);
//         USettuper.UMC(Path.Combine(privateDir, Name!) + "Core.cpp", Name!);
//         USettuper.ULH(Path.Combine(privateDir, "Log") + ".h", Name!);
//         USettuper.ULC(Path.Combine(privateDir, "Log") + ".cpp", Name!);
//         USettuper.UTC(Path.Combine(privateDir, "ActorTest") + ".cpp", Name!);
//
//         USettuper.GBB(Path.Combine(projectDir, "Build") + ".bat", config, projectConfig);
//         USettuper.GCB(Path.Combine(projectDir, "Compile") + ".bat", config, projectConfig);
//         USettuper.GCoB(Path.Combine(projectDir, "Cook") + ".bat",config, projectConfig);
//         USettuper.GEB(Path.Combine(projectDir, "Editor") + ".bat", config, projectConfig);
//
//
//         USettuper.UDEI(Path.Combine(configDir, "DefaultEngine") + ".ini", Name!);
//
//         if (AutoBuild)
//         {
//             Console.WriteLine($"\nThe first build of the {Name!} project");
//             StaticMethods.ExecuteCommand($"{Path.Combine(projectDir, "Build") + ".bat"}");
//         }
//     }
// }
//
// [Verb("open", HelpText = "Open project folder in the Explorer.")]
// internal class Open : BaseVerb
// {
//     [Option('n', "name", Required = false, HelpText = "Project name. It is not necessary if the shell is already in the desired folder.")]
//     public string? Name { get; set; }
//
//     internal override void Run()
//     {
//         if (Name is null)
//         {
//             System.Diagnostics.Process.Start("explorer.exe", $".");
//             return;
//         }
//
//         DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
//         FileInfo[] files = projectsDir.GetFiles();
//         foreach (FileInfo file in files)
//         {
//             if (Name == file.Name.Replace($".config.json", ""))
//             {
//                 USettuperProjectConfig? projectConfig = JsonSerializer.Deserialize<USettuperProjectConfig>(File.ReadAllText(appDir + @"Projects\" + $"{Name}.config.json"));
//                 if (projectConfig != null && projectConfig.ProjectDir != null)
//                     System.Diagnostics.Process.Start("explorer.exe", $"{projectConfig.ProjectDir}");
//                 Output.Succses("OK!");
//                 return;
//             }
//         }
//         Output.Error($"There is no such project {Name}");
//     }
//
// }
//
// [Verb("run", HelpText = "Run .exe standalone file.")]
// internal class RunVerb : BaseVerb
// {
//     [Option('n', "name", Required = false, HelpText = "Project name. It is not necessary if the shell is already in the desired folder.")]
//
//     public string? Name { get; set; }
//     internal override void Run()
//     {
//         if (Name is null)
//         {
//             var projectDir = new DirectoryInfo(Directory.GetCurrentDirectory());
//             if(Directory.Exists(projectDir.FullName + @"\Binaries\Win64\"))
//             {
//                 if (!File.Exists($"{projectDir.FullName}" + @"\Binaries\Win64\" + $"{projectDir.Name}.exe"))
//                 {
//                     Output.Error("Compile the project first!");
//                     return;
//                 }
//                 System.Diagnostics.Process.Start(projectDir.FullName + @"\Binaries\Win64\" + $"{projectDir.Name}.exe");
//             }
//             Output.Error("There is no project here.");
//             return;
//         }
//
//         DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
//         FileInfo[] files = projectsDir.GetFiles();
//         foreach (FileInfo file in files)
//         {
//             if (Name == file.Name.Replace($".config.json", ""))
//             {
//                 USettuperProjectConfig? projectConfig = JsonSerializer.Deserialize<USettuperProjectConfig>(File.ReadAllText(appDir + @"Projects\" + $"{Name}.config.json"));
//                 if (projectConfig != null && projectConfig.ProjectDir != null)
//                 {
//                     if (!File.Exists($"{projectConfig.ProjectDir}" + @"\Binaries\Win64\" + $"{projectConfig.Name}.exe"))
//                     {
//                         Output.Error("Compile the project first");
//                         return;
//                     }
//
//                     System.Diagnostics.Process.Start($"{projectConfig.ProjectDir}" + @"\Binaries\Win64\" + $"{projectConfig.Name}.exe");
//                 }
//                 Output.Succses("OK!");
//                 return;
//             }
//         }
//         Output.Error("There is no such project.");
//     }
// }
//
//
// // TODO: Create a separate abstract class for verbs using .bat files
// internal abstract class VerbBat : BaseVerb
// {
//     [Option('n', "name", Required = false, HelpText = "Project name. It is not necessary if the shell is already in the desired folder.")]
//     public string? Name { get; set; }
//
//     internal override void Run()
//     {
//         if (Name is null)
//         {
//             var projectDir = new DirectoryInfo(Directory.GetCurrentDirectory());
//
//             if (File.Exists($"{projectDir.FullName}" + @"\" + $"{GetType()}.bat"))
//             {
//                 StaticMethods.ExecuteCommand($"{projectDir.FullName}" + @"\" + $"{GetType()}.bat");
//                 Output.Succses("OK!");
//                 return;
//             }
//
//             Output.Error("There is no project here.");
//             return;
//         }
//
//         StaticMethods.BatRunOutput(appDir, Name!, $"{GetType()}");
//     }
//
// }
//
//
// [Verb("build", HelpText = "Build Unreal project.")]
// internal class Build : VerbBat
// {
//
// }
//
// [Verb("editor", HelpText = "Launch editor.")]
// internal class Editor : VerbBat
// {
//
// }
//
// [Verb("compile", HelpText = "Build a standalone version of project.")]
// internal class Compile : VerbBat
// {
// }
//
// [Verb("cook", HelpText = "Cook content of project.")]
// internal class Cook : VerbBat
// {
//
// }
//
// [Verb("link", HelpText = "Linking an existing unreal project with uepme.")]
// internal class Link : BaseVerb
// {
//     [Option('p', "path", Required = false, HelpText = "Full path to the project.")]
//     public string? ProjectPath { get; set; }
//     internal override void Run()
//     {
//         if (ProjectPath is null)
//         {
//             var projectDir = new DirectoryInfo(Directory.GetCurrentDirectory());
//             ProjectPath = projectDir.FullName;
//         }
//
//         USettuperConfig? config = JsonSerializer.Deserialize<USettuperConfig>(File.ReadAllText(appDir + "UnrealSettuper.config.json"));
//         if (config == null || config.ProjectsDir == null || config.UnrealDir == null)
//         {
//             Output.Error("Config error!");
//             return;
//         }
//
//         DirectoryInfo projectsDir = new DirectoryInfo(ProjectPath!);
//         FileInfo[] files = projectsDir.GetFiles();
//         bool wasFoundProjectBat = false;
//         bool wasFoundUnrealProjectFile = false;
//
//         string unrealProjectName = string.Empty;
//         foreach (FileInfo file in files)
//         {
//             if ("Build.bat" == file.Name)
//             {
//                 wasFoundProjectBat = true;
//             }
//
//             if (file.Name.Contains(".uproject"))
//             {
//                 wasFoundUnrealProjectFile = true;
//                 unrealProjectName = Path.GetFileNameWithoutExtension(ProjectPath + @"\" + $"{file.Name}");
//                 Console.WriteLine(unrealProjectName);
//             }
//         }
//         if (!wasFoundUnrealProjectFile)
//         {
//             Output.Error("No unreal files was found.");
//             return;
//         }
//
//         USettuperProjectConfig projectConfig = new USettuperProjectConfig
//         {
//             Name = unrealProjectName,
//             ProjectDir = ProjectPath
//         };
//         if (!wasFoundProjectBat)
//         {
//             Console.WriteLine("Start generating uepme build files");
//             USettuper.GBB(Path.Combine(ProjectPath!, "Build") + ".bat", config, projectConfig);
//             USettuper.GCB(Path.Combine(ProjectPath!, "Compile") + ".bat", config, projectConfig);
//             USettuper.GCoB(Path.Combine(ProjectPath!, "Cook") + ".bat", config, projectConfig);
//             USettuper.GEB(Path.Combine(ProjectPath!, "Editor") + ".bat", config, projectConfig);
//         }
//
//         string fileName = appDir + @"Projects\" + $"{unrealProjectName}.config.json";
//         string configToJson = JsonSerializer.Serialize(projectConfig);
//         File.WriteAllText(fileName, configToJson);
//         Output.Succses("Ok!");
//     }
// }
//
// [Verb("list", HelpText = "Print all uepme projects.")]
// internal class List : BaseVerb
// {
//     internal override void Run()
//     {
//         Console.WriteLine("");
//         DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
//         FileInfo[] files = projectsDir.GetFiles();
//         int i = 0;
//         foreach (FileInfo file in files)
//         {
//             i++;
//             Console.WriteLine($"{i,3}. " + $"{file.Name.Replace($".config.json", ""),10}" + $"\t{file.LastWriteTime,25}");
//         }
//
//         Console.WriteLine();
//     }
// }
//
// [Verb("delete", HelpText = "Delete the uepme project configuration file")]
// internal class Delete : BaseVerb
// {
//     [Option('n', "name", Required = true, HelpText = "Project name")]
//     public string? Name { get; set; }
//     internal override void Run()
//     {
//
//         DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
//         FileInfo[] files = projectsDir.GetFiles();
//         bool wasFound = false;
//         foreach (FileInfo file in files)
//         {
//             if (Name == file.Name.Replace($".config.json", ""))
//             {
//                 wasFound = true;
//             }
//         }
//         if (!wasFound)
//         {
//             Output.Error("There is no such project");
//             return;
//         }
//
//         Console.WriteLine("All project files will remain in place.");
//         Output.Error("Delete the configuration file?");
//
//         Console.Write("\nYes or No : ");
//
//         string? userChoose = Console.ReadLine();
//         if (userChoose == null || !userChoose.ToLower().StartsWith("y") && userChoose.ToLower().StartsWith("n"))
//         {
//             return;
//         }
//         if (userChoose.ToLower().StartsWith("y"))
//         {
//             File.Delete(appDir + @"Projects\" + $"{Name}.config.json");
//             Output.Succses("The project has been deleted");
//         }
//     }
// }