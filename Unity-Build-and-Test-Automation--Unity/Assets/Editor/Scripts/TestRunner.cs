using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

public static class TestRunner
{

    [MenuItem("Testing/Run Android Tests")]
    public static void RunAndroidTests()
    {
        var testRunner = ScriptableObject.CreateInstance<TestRunnerApi>();
        var filter = new Filter()
        {
            targetPlatform = BuildTarget.Android,
            testMode = TestMode.PlayMode
        };
        var executionSettings = new ExecutionSettings()
        {
            filters = new[] { filter }
        };
        testRunner.Execute(executionSettings);
    }
    
}
