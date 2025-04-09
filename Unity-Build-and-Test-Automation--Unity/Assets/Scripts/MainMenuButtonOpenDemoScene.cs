using UnityEngine;

public class MainMenuButtonOpenDemoScene : AButtonAction
{

    public string demoSceneIndex;
    
    public override void OnButtonClick()
    {
        SceneLoader.Instance.LoadDemoScene(demoSceneIndex);
    }
    
}
