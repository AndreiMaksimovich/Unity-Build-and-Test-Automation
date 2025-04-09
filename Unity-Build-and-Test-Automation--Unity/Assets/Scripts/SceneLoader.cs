using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public bool IsSceneLoadInProgress { get; private set; }
    
    public void LoadDemoScene(string index)
    {
        LoadScene($"Scenes/Demo.{index}");
    }

    private void LoadScene(object sceneKey)
    {
        if (IsSceneLoadInProgress)
        {
            throw new System.Exception("Scene loading is already in progress");
        }
        IsSceneLoadInProgress = true;
        var task = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Single, SceneReleaseMode.ReleaseSceneWhenSceneUnloaded);
        task.Completed += (x) =>
        {
            IsSceneLoadInProgress = false;
        };
    }

    public void LoadMainMenuScene()
    {
        if (IsSceneLoadInProgress)
        {
            throw new System.Exception("Scene loading is already in progress");
        }
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
    
}
