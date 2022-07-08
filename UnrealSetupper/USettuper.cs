using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
namespace UnrealSetupper
{
    internal static class Output
    {
        internal static void Succses(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    internal class USettuperConfig
    {
        public string? UnrealDir { get; set; }
        public string? ProjectsDir { get; set; }
    }

    internal class USettuperProjectConfig
    {
        public string? Name { get; set; }
        public string? ProjectDir { get; set; }
    }

    internal static class USettuper
    {
        public static void Config(string? unrealDir, string? projectsDir)
        {
            USettuperConfig config = new USettuperConfig
            {
                UnrealDir = unrealDir,
                ProjectsDir = projectsDir
            };
            string fileName = "UnrealSettuper.config.json";
            string configToJson = JsonSerializer.Serialize(config);
            File.WriteAllText(fileName, configToJson);

            //Console.Write(File.ReadAllText(fileName));
        }
        /// <summary>
        /// Project .uproject file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void UUPROJ(string filePath,string projectName)
        {
            try
            {
                Console.Write("\nCreating .uproject file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"{
    ""FileVersion"": 3,
    ""EngineAssociation"": ""4.27"",
    ""Category"": """",
    ""Description"": """",
    ""Modules"": [
        {
            ""Name"": " + $"\"{projectName}Core\"" + "," + @"
            ""Type"": ""Runtime"",
            ""LoadingPhase"": ""Default""
        }
    ]
}");
                }
                Output.Succses("OK!");
            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Unreal Module Target
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        /// <param name="targetType"></param>
        public static void UMT(string filePath, string projectName, string targetType)
        {
            string className = string.Empty;
            if(targetType == "Editor")
            {
                className = targetType;
            }
            try
            {
                Console.Write("\nCreating Unreal Module Target files...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"using UnrealBuildTool;
public class " + projectName + className + @"Target : TargetRules
{
    public " + projectName + className + @"Target(TargetInfo Target) : base(Target)
    {
        Type = TargetType." + targetType + @";
        DefaultBuildSettings = BuildSettingsVersion.V2;
        ExtraModuleNames.AddRange(new string[] { " + $"\"{projectName}Core\"" + @" });
    }
}");
                }
                Output.Succses("OK!");
            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Unreal Module Build
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void UMB(string filePath, string projectName)
        {
            try
            {
                Console.Write("\nCreating Unreal Module Build...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"using UnrealBuildTool;
public class " + $"{projectName}" + @"Core : ModuleRules
{
	public "+$"{projectName}"+@"Core(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
		bEnforceIWYU = true;
		PublicDependencyModuleNames.AddRange(new string[] {" + $" \"Core\", \"CoreUObject\", \"Engine\" " + @"});
        PrivateDependencyModuleNames.AddRange(new string[] { });
    }
}");
                }
                Output.Succses("OK!");
            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Unreal Module Header
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void UMH(string filePath, string projectName)
        {
            try
            {
                Console.Write("\nCreating Unreal Module Header...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"#pragma once
#include ""CoreMinimal.h""
#include ""Modules/ModuleInterface.h""
class F" + $"{projectName}" + @"Core : public IModuleInterface
{
public:
	static inline F" +$"{projectName}" +@"Core& Get()
    {
        return FModuleManager::LoadModuleChecked<F"+$"{projectName}"+ @"Core>(" + $"\"{projectName}Core\"" + @");
    }

     static inline bool IsAvailable()
    {
        return FModuleManager::Get().IsModuleLoaded(" + $"\"{projectName}Core\"" + @");
    }

    virtual void StartupModule() override;
    virtual void ShutdownModule() override;
};");
                }
                Output.Succses("OK!");
            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Unreal Module .cpp Primary
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void UMC(string filePath, string projectName)
        {
            try
            {
                Console.Write("\nCreating Unreal Module .cpp Primary...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"#include " + $"\"{projectName}Core.h\"" + @"
#include ""Modules/ModuleManager.h""
#include ""Log.h""
void F" + $"{projectName}" + @"Core::StartupModule()
{
    UE_LOG(Log" + $"{projectName}" + @"Core, Log, TEXT("" " + $"{projectName}Core " + @"module starting up""));
}

void F" + $"{projectName}" + @"Core::ShutdownModule()
{
    UE_LOG(Log" + $"{projectName}" + @"Core, Log, TEXT("" " + $"{projectName}Core " + @"module shutting down""));
}

IMPLEMENT_PRIMARY_GAME_MODULE(F" + $"{projectName}" + @"Core, " + $"{projectName}" + @"Core, " + $"\"{projectName}Core\"" + @"); ");
                }
                Output.Succses("OK!");

            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }

        /// <summary>
        /// Unreal Log Header
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void ULH(string filePath, string projectName)
        {
            try
            {
                Console.Write("\nSetting up the Ureal Log library...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"#pragma once
#include ""Logging/LogMacros.h""

DECLARE_LOG_CATEGORY_EXTERN(Log" + $"{projectName}" +@"Core, All, All);");
                }
                Output.Succses("OK!");

            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Unreal Log .cpp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void ULC(string filePath, string projectName)
        {
            try
            {
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"#include ""Log.h""
DEFINE_LOG_CATEGORY(Log" + $"{projectName}" + @"Core);
                    ");
                }

            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Generate Build Bat file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void GBB(string filePath, string projectName, USettuperConfig config)
        {
            try
            {
                Console.Write("\nGenerating batch Build file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"@echo off" + $"\n\"{config.UnrealDir}" + @"\Engine\Build\BatchFiles\Build.bat"" " + $"{projectName}Editor Win64 Development " + $"\"{config.ProjectsDir}" + @"\" + $"{projectName}" + @"\" + $"{projectName}.uproject\" -waitmutex -NoHotReload");
                }
                Output.Succses("OK!");

            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Generate Editor Bat file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        /// <param name="config"></param>
        public static void GEB(string filePath, string projectName, USettuperConfig config)
        {
            try
            {
                Console.Write("\nGenerating batch Editor file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"@echo off" + $"\ncall \"{config.UnrealDir}" + @"\Engine\Binaries\Win64\UE4Editor.exe "" " + $"\"{config.ProjectsDir}" + @"\" + $"{projectName}" + @"\" + $"{projectName}.uproject\" %*");
                }
                Output.Succses("OK!");

            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
    }
}
