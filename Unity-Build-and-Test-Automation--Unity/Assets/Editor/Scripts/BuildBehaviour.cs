using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildBehaviour", menuName = "Build/Build Behaviour")]
public abstract class BuildBehaviour : ScriptableObject
{
    public virtual void Configure(BuildConfiguration buildConfiguration, BuildPlayerOptions buildPlayerOptions) { }
    public virtual void OnProjectBuildFinished(BuildConfiguration buildConfiguration, BuildPlayerOptions buildPlayerOptions, BuildReport buildReport) { }
    public virtual void OnError(Exception exception, BuildConfiguration buildConfiguration) { }
}
