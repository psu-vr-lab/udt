// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;
// using UnrealSetupper;
//
// internal class StaticMethods
// {
//     internal static void ExecuteCommand(string command)
//     {
//         System.Diagnostics.Process process;
//         System.Diagnostics.ProcessStartInfo processStartInfo;
//         processStartInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + command);
//         processStartInfo.CreateNoWindow = false;
//         process = System.Diagnostics.Process.Start(command);
//         if (command.Contains("Editor"))
//         {
//             return;
//         }
//         process.WaitForExit();
//
//         int exitCode = process.ExitCode;
//         process.Close();
//         Console.WriteLine($"ExitCode {exitCode}");
//     }
//
//     internal static void BatRunOutput(string appDir,string name, string batFile)
//     {
//         Console.WriteLine($"Launching the {batFile} of Project {name}");
//         string projectName = name + ".config.json";
//         DirectoryInfo projectsDir = new DirectoryInfo(appDir + @"Projects");
//         FileInfo[] files = projectsDir.GetFiles();
//         foreach (FileInfo file in files)
//         {
//             if (file.Name == projectName)
//             {
//                 USettuperProjectConfig? projectConfig = JsonSerializer.Deserialize<USettuperProjectConfig>(File.ReadAllText(appDir + @"Projects\" + $"{name}" + ".config.json"));
//                 if (projectConfig != null && projectConfig.ProjectDir != null)
//                     ExecuteCommand($"{Path.Combine(projectConfig.ProjectDir, batFile) + ".bat"}");
//                 Output.Succses("OK!");
//                 return;
//             }
//         }
//         Output.Error("There is no such project");
//     }
//
//     internal static string RunTerminalCommand(string programName, string args)
//     {
//         Process p = new Process();
//         // Redirect the output stream of the child process.
//         p.StartInfo.UseShellExecute = false;
//         p.StartInfo.RedirectStandardOutput = true;
//         p.StartInfo.FileName = "cmd.exe";
//         p.StartInfo.Arguments = $"/C {programName} {args}";
//         p.Start();
//         // Read the output stream first and then wait.
//         string output = p.StandardOutput.ReadToEnd();
//         p.WaitForExit();
//
//         return output;
//     }
// }
