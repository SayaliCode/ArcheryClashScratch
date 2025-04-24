using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreen
{
    public int GetScreenId();
    public int GetScreenLayer();

    public void UpdateScreenParameters(ScreenParameters screenParameters);

    public void DestroyScreen();

    public void Init(ScreenPrefabList<int> screenData, ScreenParameters screenParameters);

    public void OnBackButtonClicked();

    public void OnClickOnButton(string clickEvent);

    public GameObject GetScreenObject();
}

[System.Serializable]
public class ScreenParameters
{
    public List<object> parameters;
    public ScreenController controller;

    public ScreenParameters(List<object> obj)
    {
        parameters = obj;
    }

    public ScreenParameters()
    {

    }
}
