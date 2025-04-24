using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;

public class MainMenuScreensController : ScreenController
{
    public static MainMenuScreensController instance;
    private void Start()
    {
        instance = this;
        Init(PrefabsHolder.GetInstance.GetMainMenuScreens());
        UpdateCanvas();
        ShowScreen(MainMenuScreen.LoadingScreen);
    }

    // Start is called before the first frame update
    public void ShowScreen(MainMenuScreen m_ScreenId, ScreenParameters m_ScreenParameters = null)
    {
        ShowScreen((int)m_ScreenId, m_ScreenParameters);
    }

    public virtual void DestroyScreen(MainMenuScreen m_ScreenId)
    {
        DestroyScreen((int)m_ScreenId);
    }
}
