using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LightBaker
{
    private const string ConfigurationAssetPath = "Assets/Configurations/LightBaker.asset";

    [InitializeOnLoadMethod]
    private static void OnInitialized()
    {
        var configuration = Configuration;
    }
    
    private static LightBakerConfiguration Configuration =>
        ConfigurationScriptableObjectUtils.GetInstance<LightBakerConfiguration>("LightBakerConfiguration");
    
    
    [MenuItem("LightBaker/Bake Lighting")]
    public static int BakeLighting()
    {
        var configuration = Configuration;
        Debug.Log(JsonUtility.ToJson(configuration));
        return BakeLight(configuration.bakeLightingInScenes) ? 0 : 1;
    }
    
    public static bool BakeLight(List<SceneAsset> scenes)
    {
        var scenePaths = 
            scenes
                .Where(scene => scene != null)
                .Select(AssetDatabase.GetAssetPath)
                .ToList();
        return RunActionOnScenes(
            scenePaths, 
            () => Lightmapping.Bake(), 
            "Bake Lighting");
    }
    
    private static bool RunActionOnScenes (List<string> scenePaths, Action action, string label = "")
    {
        try
        {
            var currentScenePath = SceneManager.GetActiveScene().path;
            EditorSceneManager.SaveOpenScenes();

            var i = 0f;

            foreach (var scenePath in scenePaths)
            {
                Debug.Log($"{label}: {scenePath} {i+1}/{scenePaths.Count}");
                EditorUtility.DisplayProgressBar(label, scenePath, i / (float)scenePaths.Count);
                EditorSceneManager.OpenScene(scenePath);
                action.Invoke();
                EditorSceneManager.SaveOpenScenes();
                i++;
            }

            if (!string.IsNullOrEmpty(currentScenePath))
            {
                EditorSceneManager.OpenScene(currentScenePath);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        return true;
    }
    
}
