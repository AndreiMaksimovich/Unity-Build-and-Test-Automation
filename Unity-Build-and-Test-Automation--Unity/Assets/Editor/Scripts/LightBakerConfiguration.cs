using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LightBakerConfiguration", menuName = "Light Baker/Configuration")]
public class LightBakerConfiguration : ScriptableObject
{
    public List<SceneAsset> bakeLightingInScenes = new();
}
