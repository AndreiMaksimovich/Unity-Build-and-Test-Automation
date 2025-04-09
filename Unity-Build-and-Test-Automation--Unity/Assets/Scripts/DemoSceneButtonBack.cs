public class DemoSceneButtonBack : AButtonAction
{
    public override void OnButtonClick()
    {
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
