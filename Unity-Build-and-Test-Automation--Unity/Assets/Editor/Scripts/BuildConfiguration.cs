using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildConfiguration", menuName = "Build/Build Configuration")]
public class BuildConfiguration : ScriptableObject
{
    [Serializable]
    public enum Target
    {
        Android,
        iOS,
        WebGLDesktop,
        WebGLMobile
    }

    public SceneAsset[] scenes = {};
    public Target target;
    public BuildOptions buildOptions = BuildOptions.Development | BuildOptions.CompressWithLz4;
    public List<string> extraScriptingDefines;
    public List<BuildBehaviour> buildBehaviours = new ();
    
    [Header("WebGL")]
    public WebGLCompressionFormat webGLCompressionFormat = WebGLCompressionFormat.Disabled;
    public WebGLDebugSymbolMode webGLDebugSymbolMode = WebGLDebugSymbolMode.Off;
    public string webGLTemplate = "";
}
