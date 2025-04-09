using System.IO;
using UnityEditor;
using UnityEngine;

public static class ConfigurationScriptableObjectUtils
{
    
    private const string ConfigurationDir = "Assets/Configurations";

    public static T GetInstance<T>(string configurationName) where T : ScriptableObject
    {
        if (!Directory.Exists(ConfigurationDir))
        {
            Directory.CreateDirectory(ConfigurationDir);
        }

        var configFilePath = Path.Combine(ConfigurationDir, $"{configurationName}.asset");

        if (File.Exists(configFilePath))
        {
            return AssetDatabase.LoadAssetAtPath<T>(configFilePath);
        }

        var configuration = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(configuration, configFilePath);
        return configuration;
    }
    
}
