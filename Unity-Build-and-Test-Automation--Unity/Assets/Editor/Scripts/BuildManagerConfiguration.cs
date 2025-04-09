using UnityEngine;

public class BuildManagerConfiguration : ScriptableObject
{
    public string defaultWebGLTemplatePC = "PROJECT:Default";
    public string defaultWebGLTemplateMobile = "PROJECT:Default";
    public string buildConfigurationsDirectory = "Assets/Configurations/BuildConfigurations";
    public string buildsDirectory = "../Builds";
}
