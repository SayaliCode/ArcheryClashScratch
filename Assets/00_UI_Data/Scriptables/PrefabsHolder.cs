using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.UI
{
    [CreateAssetMenu(fileName = "PrefabHolder", menuName = "Scriptables/PrefabHolder")]
    public class PrefabsHolder : ScriptableObject
    {
        [SerializeField] List<ScreenPrefabList<MainMenuScreen>> mainMenuScreenPrefabList;
        [SerializeField] List<ScreenPrefabList<InGameScreen>> inGameScreensPrefabList;
        public GameObject toastMessagePrefab;
        public GameObject calendarPrefab;


        private static PrefabsHolder _prefabHolder;

        public static PrefabsHolder GetInstance
        {
            get
            {
                if(_prefabHolder == null)
                {
                    _prefabHolder = Resources.Load<PrefabsHolder>("PrefabHolder");
                }

                return _prefabHolder;
            }
        }


        public List<ScreenPrefabList<int>> GetMainMenuScreens()
        {
            List<ScreenPrefabList<int>> prefabsList = new List<ScreenPrefabList<int>>();

            for (int i = 0; i < mainMenuScreenPrefabList.Count; i++)
            {
                prefabsList.Add(new ScreenPrefabList<int>(mainMenuScreenPrefabList[i].screenPrefab, (int)mainMenuScreenPrefabList[i].screenId, mainMenuScreenPrefabList[i].spawnLayer));
            }

            return prefabsList;
        }

        public List<ScreenPrefabList<int>> GetInGameScreens()
        {
            List<ScreenPrefabList<int>> prefabsList = new List<ScreenPrefabList<int>>();

            for (int i = 0; i < inGameScreensPrefabList.Count; i++)
            {
                prefabsList.Add(new ScreenPrefabList<int>(inGameScreensPrefabList[i].screenPrefab, (int)inGameScreensPrefabList[i].screenId, inGameScreensPrefabList[i].spawnLayer));
            }

            return prefabsList;
        }

        
    }


    public enum MainMenuScreen
	{
        LoadingScreen = 0,
        MainLobby = 1
    }

    public enum InGameScreen
    {

    }
}