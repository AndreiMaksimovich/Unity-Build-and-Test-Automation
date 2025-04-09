using UnityEngine;

public class MainMenuButtonExit : AButtonAction
{
    
    public override void OnButtonClick()
    {
        Application.Quit();
    }
    
}
