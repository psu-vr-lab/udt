using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
namespace UnrealSetupper
{
    internal class USettuperConfig
    {
        public string? UnrealDir { get; set; }
        public string? ProjectsDir { get; set; }
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
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"{
    ""FileVersion"": 3,
    ""EngineAssociation"": ""4.27"",
    ""Category"": """",
    ""Description"": """",
    ""Modules"": [
        {
            ""Name"":" + $"\"{projectName}Core\"" + "," + @"
            ""Type"": ""Runtime"",
            ""LoadingPhase"": ""Default""
        }
    ]
}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
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
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"using UnrealBuildTool;
public class " + $"{projectName}" + @"Core : ModuleRules
{
	public BobCore(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
		bEnforceIWYU = true;
		PublicDependencyModuleNames.AddRange(new string[] {" + $" \"Core\", \"CoreUObject\", \"Engine\" " + @"});
        PrivateDependencyModuleNames.AddRange(new string[] { });

    }
}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
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
            return FModuleManager::LoadModuleChecked<F"+$"{projectName}"+ @"Core>(" + $"{projectName}" + @");
        }

        static inline bool IsAvailable()
        {
            return FModuleManager::Get().IsModuleLoaded(" + $"{projectName}" + @");
        }

        virtual void StartupModule() override;
	    virtual void ShutdownModule() override;
};");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

    }
}
