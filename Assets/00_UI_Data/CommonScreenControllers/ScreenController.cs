using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [SerializeField] private List<ScreenPrefabList<int>> _listOfAllScreenPrefabs;
    [SerializeField] Transform[] _screenLayers;
    [SerializeField] List<IScreen> _spawnedScreens = new List<IScreen>();
    [SerializeField] Canvas canvas;


    protected virtual void Init(List<ScreenPrefabList<int>> screenPrefabList)
    {
        _listOfAllScreenPrefabs = screenPrefabList;
    }

    protected virtual void UpdateCanvas()
    {
        CanvasScaler canvasScaller = canvas.GetComponent<CanvasScaler>();
        float defaultScreenFactor = canvasScaller.referenceResolution.x / canvasScaller.referenceResolution.y;//1.91
        float currentScreenFactor = (float)Screen.width / Screen.height;

        if (currentScreenFactor > defaultScreenFactor)
        {
            canvasScaller.matchWidthOrHeight = 1f;
        }
        else
        {
            canvasScaller.matchWidthOrHeight = 0f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickOnBackButton();
        }
    }

    private void RemoveNullObjects()
    {
        for (int i = 0; i < _spawnedScreens.Count; i++)
        {
            if (_spawnedScreens == null)
            {
                _spawnedScreens.RemoveAt(i);
                --i;
            }
        }
    }


    public virtual void OnClickOnBackButton()
    {
        RemoveNullObjects();

        IScreen topLayerScreen = null;

        for (int i = 0; i < _spawnedScreens.Count; i++)
        {
            if (_spawnedScreens != null)
            {
                if (topLayerScreen == null)
                {
                    topLayerScreen = _spawnedScreens[i];
                }

                if (_spawnedScreens[i].GetScreenLayer() > topLayerScreen.GetScreenLayer())
                {
                    topLayerScreen = _spawnedScreens[i];
                }
            }
        }

        if (topLayerScreen != null)
        {
            topLayerScreen.OnBackButtonClicked();
        }
    }

    protected virtual void ShowScreen(int screenId, ScreenParameters screenParameters)
    {
        ScreenPrefabList<int> screenToSpawn = _listOfAllScreenPrefabs.Where(x => x.screenId == screenId).FirstOrDefault();
        ScreenValidationData screenValidationData = ValidateScreenBeforeSpawning(screenToSpawn);


        if (screenValidationData.isScreenAlreadyOpened)
        {
            screenValidationData.spawnedScreen.UpdateScreenParameters(screenParameters);
            DestroyScreensUptoLayer(screenToSpawn.spawnLayer + 1);
        }
        else
        {
            DestroyScreensUptoLayer(screenToSpawn.spawnLayer);
            SpawnScreen(screenToSpawn, screenParameters);
        }
    }

    private void SpawnScreen(ScreenPrefabList<int> screenToSpawn, ScreenParameters screenParameters)
    {
        GameObject gm = Instantiate(screenToSpawn.screenPrefab, _screenLayers[screenToSpawn.spawnLayer]);
        IScreen screen = gm.GetComponent<ScreenBehaviour>();

        if (screenParameters == null)
        {
            screenParameters = new ScreenParameters();
        }

        screenParameters.controller = this;
        screen.Init(screenToSpawn, screenParameters);
        _spawnedScreens.Add(screen);
    }

    private void DestroyScreensUptoLayer(int layerId)
    {
        RemoveNullObjects();

        for (int i = 0; i < _spawnedScreens.Count; i++)
        {
            if (_spawnedScreens[i].GetScreenLayer() >= layerId)
            {
                Destroy(_spawnedScreens[i].GetScreenObject());
                _spawnedScreens.RemoveAt(i);
                --i;
            }
        }
    }

    public virtual void DestroyScreen(int screenId)
    {
        RemoveNullObjects();

        for (int i = 0; i < _spawnedScreens.Count; i++)
        {
            if (_spawnedScreens[i].GetScreenId() == screenId)
            {
                Destroy(_spawnedScreens[i].GetScreenObject());
                _spawnedScreens.RemoveAt(i);
                break;
            }
        }
    }



    private ScreenValidationData ValidateScreenBeforeSpawning(ScreenPrefabList<int> screenPrefab)
    {
        RemoveNullObjects();

        ScreenValidationData screenValidationData = new ScreenValidationData();

        for (int i = 0; i < _spawnedScreens.Count; i++)
        {
            if (_spawnedScreens != null)
            {
                if (_spawnedScreens[i].GetScreenLayer() == screenPrefab.spawnLayer)
                {
                    screenValidationData.spawnedScreen = _spawnedScreens[i];

                    if (_spawnedScreens[i].GetScreenId() == screenPrefab.screenId)
                    {
                        screenValidationData.isScreenAlreadyOpened = true;
                    }
                    else
                    {
                        screenValidationData.isAnotherScreenOpenedOnSameLayer = true;
                    }
                }
            }
        }

        return screenValidationData;
    }

    [System.Serializable]
    private class ScreenValidationData
    {
        public bool isScreenAlreadyOpened;
        public bool isAnotherScreenOpenedOnSameLayer;
        public IScreen spawnedScreen;
    }
}


[System.Serializable]
public class ScreenPrefabList<T>
{
    public T screenId;
    public GameObject screenPrefab;
    public int spawnLayer;

    public ScreenPrefabList(GameObject m_prefab, T m_ScreenId, int m_spawnLayer)
    {
        screenId = m_ScreenId;
        screenPrefab = m_prefab;
        spawnLayer = m_spawnLayer;
    }

    public ScreenPrefabList()
    {

    }
}
