using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Reflection;

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
            string fileName = Program.appDir + "UnrealSettuper.config.json";
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
                Console.Write("\nCreating Unreal Log .cpp Primary...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"#include ""Log.h""
DEFINE_LOG_CATEGORY(Log" + $"{projectName}" + @"Core);
                    ");
                }
                Output.Succses("OK!");


            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Unreal Actor Test Header
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void UTH(string filePath, string projectName)
        {
            try
            {
                Console.Write("\nCreating Unreal Actor Test Template header file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"#pragma once
#include ""CoreMinimal.h""
#include ""GameFramework/Actor.h""
#include ""ActorTest.generated.h""
UCLASS()
class AActorTest : public AActor
{
	GENERATED_BODY()

public:
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category= ""Components"")
    class UBillboardComponent* Sprite;

public:
	AActorTest(const FObjectInitializer& ObjectInitializer);
	virtual void BeginPlay() override;
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
        /// Unreal Actor Test .cpp file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void UTC(string filePath, string projectName)
        {
            try
            {
                Console.Write("\nCreating Unreal Actor Test Template .cpp file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"#include ""ActorTest.h""
#include ""Components/SceneComponent.h""
#include ""Components/BillboardComponent.h""
#include ""Log.h""
AActorTest::AActorTest(const FObjectInitializer&ObjectInitializer) : Super(ObjectInitializer)
{
    RootComponent = ObjectInitializer.CreateDefaultSubobject<USceneComponent>(this, TEXT(""RootComponent""));
    Sprite = ObjectInitializer.CreateDefaultSubobject<UBillboardComponent>(this, TEXT(""Sprite""));
    Sprite->SetupAttachment(RootComponent);
}
void AActorTest::BeginPlay(){
    Super::BeginPlay();
    UE_LOG(Log" + $"{projectName}" + @"Core, Log, TEXT(""Hello uempe""));
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
        /// Unreal GameBase Header file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void UGBH(string filePath, string projectName)
        {
            try
            {
                Console.Write("\nCreating Unreal GameBase Template header file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"// Copyright Epic Games, Inc. All Rights Reserved.
#pragma once
#include ""CoreMinimal.h""
#include ""GameFramework/GameModeBase.h""
#include """ + $"{projectName}" + @"GameModeBase.generated.h""

UCLASS()
class " + $"{projectName.ToUpper()}" +@"_API A" + $"{projectName}" + @"GameModeBase : public AGameModeBase
{
	GENERATED_BODY()

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
        /// Unreal GameBase .cpp file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void UGBC(string filePath, string projectName)
        {
            try
            {
                Console.Write("\nCreating Unreal GameBase Template .cpp file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"#include """ + $"{projectName}" +@"GameModeBase.h""");
                }
                Output.Succses("OK!");

            }
            catch (Exception e)
            {
                Output.Error("Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Unreal Default Engine ini file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void UDEI(string filePath, string projectName)
        {
            try
            {
                Console.Write("\nCreating Unreal Default Engine ini file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"
[/Script/HardwareTargeting.HardwareTargetingSettings]
TargetedHardwareClass=Desktop
AppliedTargetedHardwareClass=Desktop
DefaultGraphicsPerformance=Scalable
AppliedDefaultGraphicsPerformance=Scalable

[/Script/Engine.RendererSettings]
r.Mobile.DisableVertexFog=True
r.Shadow.CSM.MaxMobileCascades=2
r.MobileMSAA=1
r.Mobile.AllowDitheredLODTransition=False
r.Mobile.AllowSoftwareOcclusion=False
r.Mobile.VirtualTextures=False
r.DiscardUnusedQuality=False
r.AllowOcclusionQueries=True
r.MinScreenRadiusForLights=0.030000
r.MinScreenRadiusForDepthPrepass=0.030000
r.MinScreenRadiusForCSMDepth=0.010000
r.PrecomputedVisibilityWarning=False
r.TextureStreaming=True
Compat.UseDXT5NormalMaps=False
r.VirtualTextures=False
r.VT.EnableAutoImport=True
r.VirtualTexturedLightmaps=False
r.VT.TileSize=128
r.VT.TileBorderSize=4
r.vt.FeedbackFactor=16
r.VT.EnableCompressZlib=True
r.VT.EnableCompressCrunch=False
r.ClearCoatNormal=False
r.ReflectionCaptureResolution=128
r.Mobile.ReflectionCaptureCompression=False
r.ReflectionEnvironmentLightmapMixBasedOnRoughness=True
r.ForwardShading=False
r.VertexFoggingForOpaque=True
r.AllowStaticLighting=True
r.NormalMapsForStaticLighting=False
r.GenerateMeshDistanceFields=False
r.DistanceFieldBuild.EightBit=False
r.GenerateLandscapeGIData=False
r.DistanceFieldBuild.Compress=False
r.TessellationAdaptivePixelsPerTriangle=48.000000
r.SeparateTranslucency=True
r.TranslucentSortPolicy=0
TranslucentSortAxis=(X=0.000000,Y=-1.000000,Z=0.000000)
vr.VRS.HMDFixedFoveationLevel=0
r.CustomDepth=1
r.CustomDepthTemporalAAJitter=True
r.PostProcessing.PropagateAlpha=0
r.DefaultFeature.Bloom=True
r.DefaultFeature.AmbientOcclusion=True
r.DefaultFeature.AmbientOcclusionStaticFraction=True
r.DefaultFeature.AutoExposure=False
r.DefaultFeature.AutoExposure.Method=0
r.DefaultFeature.AutoExposure.Bias=1.000000
r.DefaultFeature.AutoExposure.ExtendDefaultLuminanceRange=False
r.UsePreExposure=True
r.EyeAdaptation.EditorOnly=False
r.DefaultFeature.MotionBlur=False
r.DefaultFeature.LensFlare=False
r.TemporalAA.Upsampling=False
r.SSGI.Enable=False
r.DefaultFeature.AntiAliasing=0
r.DefaultFeature.LightUnits=1
r.DefaultBackBufferPixelFormat=4
r.Shadow.UnbuiltPreviewInGame=True
r.StencilForLODDither=False
r.EarlyZPass=3
r.EarlyZPassOnlyMaterialMasking=False
r.DBuffer=True
r.ClearSceneMethod=1
r.BasePassOutputsVelocity=False
r.VertexDeformationOutputsVelocity=False
r.SelectiveBasePassOutputs=False
bDefaultParticleCutouts=False
fx.GPUSimulationTextureSizeX=1024
fx.GPUSimulationTextureSizeY=1024
r.AllowGlobalClipPlane=False
r.GBufferFormat=1
r.MorphTarget.Mode=True
r.GPUCrashDebugging = False
vr.InstancedStereo = False
r.MobileHDR=True
vr.MobileMultiView=False
r.Mobile.UseHWsRGBEncoding=False
vr.RoundRobinOcclusion=False
vr.ODSCapture=False
r.MeshStreaming=False
r.WireframeCullThreshold=5.000000
r.RayTracing=False
r.RayTracing.UseTextureLod=False
r.SupportStationarySkylight=True
r.SupportLowQualityLightmaps=True
r.SupportPointLightWholeSceneShadows=True
r.SupportAtmosphericFog=True
r.SupportSkyAtmosphere=True
r.SupportSkyAtmosphereAffectsHeightFog=False
r.SkinCache.CompileShaders=False
r.SkinCache.DefaultBehavior=1
r.SkinCache.SceneMemoryLimitInMB=128.000000
r.Mobile.EnableStaticAndCSMShadowReceivers=True
r.Mobile.EnableMovableLightCSMShaderCulling=True
r.Mobile.AllowDistanceFieldShadows=True
r.Mobile.AllowMovableDirectionalLights=True
r.MobileNumDynamicPointLights=4
r.MobileDynamicPointLightsUseStaticBranch=True
r.Mobile.EnableMovableSpotlights=False
r.Mobile.EnableMovableSpotlightsShadow=False
r.GPUSkin.Support16BitBoneIndex=False
r.GPUSkin.Limit2BoneInfluences=False
r.SupportDepthOnlyIndexBuffers=True
r.SupportReversedIndexBuffers=True
r.LightPropagationVolume=False
r.Mobile.AmbientOcclusion=False
r.GPUSkin.UnlimitedBoneInfluences=False
r.GPUSkin.UnlimitedBoneInfluencesThreshold=8
MaxSkinBones=(Default=65536,PerPlatform=((""Mobile"",256)))
r.Mobile.PlanarReflectionMode=0
r.Mobile.SupportsGen4TAA=False
bStreamSkeletalMeshLODs=(Default=False,PerPlatform=())
bDiscardSkeletalMeshOptionalLODs=(Default=False,PerPlatform=())
VisualizeCalibrationColorMaterialPath=/Engine/EngineMaterials/PPM_DefaultCalibrationColor.PPM_DefaultCalibrationColor
VisualizeCalibrationCustomMaterialPath=None
VisualizeCalibrationGrayscaleMaterialPath=/Engine/EngineMaterials/PPM_DefaultCalibrationGrayscale.PPM_DefaultCalibrationGrayscale
");
                }
                Output.Succses("OK!");

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
        /// Generate Compile Bat file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        public static void GCB(string filePath, string projectName, USettuperConfig config)
        {
            try
            {
                Console.Write("\nGenerating batch Compile file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"@echo off" + $"\n\"{config.UnrealDir}" + @"\Engine\Build\BatchFiles\Build.bat"" " + $"{projectName} Win64 Development " + $"\"{config.ProjectsDir}" + @"\" + $"{projectName}" + @"\" + $"{projectName}.uproject\" -waitmutex -NoHotReload");
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
        /// <summary>
        /// Generate Cook Bat file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectName"></param>
        /// <param name="config"></param>
        public static void GCoB(string filePath, string projectName, USettuperConfig config)
        {
            try
            {
                Console.Write("\nGenerating batch Cook file...");
                using (StreamWriter sw = File.CreateText(Path.Combine(filePath)))
                {
                    sw.WriteLine(@"@echo off" + $"\ncall \"{config.UnrealDir}" + @"\Engine\Binaries\Win64\UE4Editor-cmd.exe "" " + $"\"{config.ProjectsDir}" + @"\" + $"{projectName}" + @"\" + $"{projectName}.uproject\" -run=cook -targetplatform=WindowsNoEditor");
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
