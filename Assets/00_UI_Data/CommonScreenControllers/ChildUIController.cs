using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChildUIController<T>
{
    [SerializeField] List<ChildUI> childUI;

    public void Show(T screenId)
    {
        for (int i = 0; i < childUI.Count; i++)
        {
            childUI[i].ToggleScreen(childUI[i].screenId.Equals(screenId));
        }
    }

    public void HideAll()
    {
        for (int i = 0; i < childUI.Count; i++)
        {
            childUI[i].ToggleScreen(false);
        }
    }

    public ChildUI GetActiveScreen()
    {
        ChildUI activeChildUI = null;

        for (int i = 0; i < childUI.Count; i++)
        {
            if (childUI[i].IsScreenActive())
            {
                activeChildUI = childUI[i];
            }
        }

        return activeChildUI;
    }

    [System.Serializable]
    public class ChildUI
    {
        public T screenId;
        public GameObject screenObj;

        public void ToggleScreen(bool isShow)
        {
            screenObj.SetActive(isShow);
        }

        public bool IsScreenActive()
        {
            return screenObj.activeInHierarchy;
        }
    }
}
