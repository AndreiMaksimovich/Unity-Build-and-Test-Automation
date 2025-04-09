using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildManager
{
    [InitializeOnLoadMethod]
    private static void OnInitialized()
    {
        var configuration = Configuration;
    }

    private static BuildManagerConfiguration Configuration =>
        ConfigurationScriptableObjectUtils.GetInstance<BuildManagerConfiguration>("BuildManagerConfiguration");

    private static string GetBuildPath(BuildConfiguration buildConfiguration)
    {
        var dir = buildConfiguration.target switch
        {
            BuildConfiguration.Target.Android => "Android",
            BuildConfiguration.Target.iOS => "iOS",
            BuildConfiguration.Target.WebGLDesktop => "WebGL/Desktop",
            BuildConfiguration.Target.WebGLMobile => "WebGL/Mobile",
            _ => throw new ArgumentOutOfRangeException(nameof(buildConfiguration), buildConfiguration, null)
        };
        return Path.Combine(Configuration.buildsDirectory, dir);
    }

    private static bool IsWebGLBuild(BuildConfiguration buildConfiguration) =>
        buildConfiguration.target is BuildConfiguration.Target.WebGLMobile or BuildConfiguration.Target.WebGLDesktop;

    private static string GetWebGLTemplate(BuildConfiguration buildConfiguration)
    {
        if (!string.IsNullOrEmpty(buildConfiguration.webGLTemplate))
        {
            return buildConfiguration.webGLTemplate;
        }
        return buildConfiguration.target == BuildConfiguration.Target.WebGLDesktop ? Configuration.defaultWebGLTemplatePC : Configuration.defaultWebGLTemplateMobile;
    }

    private static WebGLTextureSubtarget GetWebGLTextureSubtarget(BuildConfiguration.Target target)
        => target switch
        {
            BuildConfiguration.Target.WebGLMobile => WebGLTextureSubtarget.ETC2,
            BuildConfiguration.Target.WebGLDesktop => WebGLTextureSubtarget.DXT,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    private static void SwitchActiveBuildTarget(BuildConfiguration buildConfiguration)
    {
        switch (buildConfiguration.target)
        {
            case BuildConfiguration.Target.Android:
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                break;
            case BuildConfiguration.Target.iOS:
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
                break;
            case BuildConfiguration.Target.WebGLMobile:
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
                break;
            case BuildConfiguration.Target.WebGLDesktop:
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static string[] GetExtraScriptingDefines(BuildConfiguration buildConfiguration)
    {
        return buildConfiguration.extraScriptingDefines.ToArray();
    }

    private static BuildTarget GetBuildTarget(BuildConfiguration buildConfiguration)
        => buildConfiguration.target switch
        {
            BuildConfiguration.Target.WebGLDesktop => BuildTarget.WebGL,
            BuildConfiguration.Target.WebGLMobile => BuildTarget.WebGL,
            BuildConfiguration.Target.Android => BuildTarget.Android,
            BuildConfiguration.Target.iOS => BuildTarget.iOS,
            _ => throw new ArgumentOutOfRangeException()
        };
    
    private static BuildReport Build(BuildConfiguration buildConfiguration)
    {
        try
        {
            SwitchActiveBuildTarget(buildConfiguration);

            if (IsWebGLBuild(buildConfiguration))
            {
                PlayerSettings.WebGL.template = GetWebGLTemplate(buildConfiguration);
                EditorUserBuildSettings.webGLBuildSubtarget = GetWebGLTextureSubtarget(buildConfiguration.target);
                PlayerSettings.WebGL.debugSymbolMode = buildConfiguration.webGLDebugSymbolMode;
                PlayerSettings.WebGL.compressionFormat = buildConfiguration.webGLCompressionFormat;
                Debug.Log(PlayerSettings.WebGL.compressionFormat);
            }

            var mainBuildPath = GetBuildPath(buildConfiguration);

            if (Directory.Exists(mainBuildPath))
            {
                Directory.Delete(mainBuildPath, true);
            }
            else
            {
                Directory.CreateDirectory(mainBuildPath);
            }

            if (buildConfiguration.target == BuildConfiguration.Target.Android)
            {
                mainBuildPath = Path.Combine(mainBuildPath, "Android.apk");
            }

            var buildPlayerOptions = new BuildPlayerOptions
            {
                locationPathName = mainBuildPath,
                target = GetBuildTarget(buildConfiguration),
                extraScriptingDefines = GetExtraScriptingDefines(buildConfiguration),
                options = buildConfiguration.buildOptions,
                scenes = buildConfiguration.scenes.Select(AssetDatabase.GetAssetPath).ToArray(),
            };

            foreach (var buildBehaviour in buildConfiguration.buildBehaviours)
            {
                buildBehaviour.Configure(buildConfiguration, buildPlayerOptions);
            }

            // Refresh Asset Database

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Build Addressables
            AddressableAssetSettings.CleanPlayerContent(UnityEditor.AddressableAssets
                .AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
            AddressableAssetSettings.BuildPlayerContent();

            // Build
            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            foreach (var buildBehaviour in buildConfiguration.buildBehaviours)
            {
                buildBehaviour.OnProjectBuildFinished(buildConfiguration, buildPlayerOptions, report);
            }
            
            return report;
        }
        catch (Exception exception)
        {
            foreach (var buildBehaviour in buildConfiguration.buildBehaviours)
            {
                buildBehaviour.OnError(exception, buildConfiguration);
            }
            throw;
        }
    }
    
    public static void BuildAddressablesWebGL()
    {
        EditorUserBuildSettings.webGLBuildSubtarget = GetWebGLTextureSubtarget(BuildConfiguration.Target.WebGLDesktop);
        BuildAddressables(BuildTargetGroup.WebGL, BuildTarget.WebGL);
    }
    
    public static void BuildAddressablesWebGLDesktop()
    {
        EditorUserBuildSettings.webGLBuildSubtarget = GetWebGLTextureSubtarget(BuildConfiguration.Target.WebGLDesktop);
        BuildAddressables(BuildTargetGroup.WebGL, BuildTarget.WebGL);
    }
    
    public static void BuildAddressablesWebGLMobile()
    {
        EditorUserBuildSettings.webGLBuildSubtarget = GetWebGLTextureSubtarget(BuildConfiguration.Target.WebGLMobile);
        BuildAddressables(BuildTargetGroup.WebGL, BuildTarget.WebGL);
    }
    
    public static void BuildAddressablesIOS()
    {
        BuildAddressables(BuildTargetGroup.iOS, BuildTarget.iOS);
    }
    
    public static void BuildAddressablesStandaloneOSX()
    {
        BuildAddressables(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
    }
    
    public static void BuildAddressablesAndroid()
    {
        BuildAddressables(BuildTargetGroup.Android, BuildTarget.Android);
    }

    public static void BuildAddressables(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        AddressableAssetSettings.CleanPlayerContent(UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
        AddressableAssetSettings.BuildPlayerContent();
    }
    
    private static void BuildWrapper(BuildConfiguration buildConfiguration)
    {
        Build(buildConfiguration);
    }

    public static void Build(string buildConfigurationFileName)
    {
        var path = Path.Combine(Configuration.buildConfigurationsDirectory, buildConfigurationFileName + ".asset");
        var buildConfiguration = AssetDatabase.LoadAssetAtPath<BuildConfiguration>(path);
        if (buildConfiguration == null)
        {
            Debug.LogError($"Failed to load build configuration file: {path}");
            return;
        }
        Build(buildConfiguration);
    }

    [MenuItem("Build/Development/WebGL Desktop")]
    public static void BuildWebGLDesktopDevelopment()
    {
        Build("WebGL.Desktop.Development");
    }
    
    [MenuItem("Build/Development/WebGL Mobile")]
    public static void BuildWebGLMobileDevelopment()
    {
        Build("WebGL.Mobile.Development");
    }
    
    [MenuItem("Build/Development/WebGL Mobile & Desktop")]
    public static void BuildWebGLMobileAndDesktopDevelopment()
    {
        BuildWebGLDesktopDevelopment();
        BuildWebGLMobileDevelopment();
    }
    
    [MenuItem("Build/Development/Android")]
    public static void BuildAndroidDevelopment()
    {
        Build("Android.Development");
    }
    
    [MenuItem("Build/Development/iOS")]
    public static void BuildIOSDevelopment()
    {
        Build("iOS.Development");
    }
    
    [MenuItem("Build/Production/WebGL Desktop")]
    public static void BuildWebGLDesktopProduction()
    {
        Build("WebGL.Desktop.Production");
    }
    
    [MenuItem("Build/Production/WebGL Mobile")]
    public static void BuildWebGLMobileProduction()
    {
        Build("WebGL.Mobile.Production");
    }
    
    [MenuItem("Build/Production/WebGL Mobile & Desktop")]
    public static void BuildWebGLMobileAndDesktopProduction()
    {
        BuildWebGLDesktopProduction();
        BuildWebGLMobileProduction();
    }
    
    [MenuItem("Build/Production/Android")]
    public static void BuildAndroidProduction()
    {
        Build("Android.Production");
    }
    
    [MenuItem("Build/Production/iOS")]
    public static void BuildIOSProduction()
    {
        Build("iOS.Production");
    }
}

