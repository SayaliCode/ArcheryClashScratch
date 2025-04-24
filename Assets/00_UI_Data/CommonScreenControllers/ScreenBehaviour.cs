using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBehaviour : MonoBehaviour, IScreen
{
    protected int screenId, screenLayer;
    private ScreenController screenController;

    public virtual void Init(ScreenPrefabList<int> screenData, ScreenParameters screenParameters)
    {
        screenId = screenData.screenId;
        screenLayer = screenData.spawnLayer;
        screenController = screenParameters.controller;
    }

    public virtual void UpdateScreenParameters(ScreenParameters screenParameters)
    {
    }

    public virtual int GetScreenId()
    {
        return screenId;
    }

    public virtual int GetScreenLayer()
    {
        return screenLayer;
    }

    public virtual GameObject GetScreenObject()
    {
        return gameObject;
    }

    public virtual void DestroyScreen()
    {
        screenController.DestroyScreen(GetScreenId());
    }

    public virtual void OnBackButtonClicked()
    {
        
    }

    public virtual void OnClickOnButton(string clickEvent)
    {
        
    }
}
