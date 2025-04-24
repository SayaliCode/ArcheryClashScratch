using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : ScreenBehaviour
{
    [SerializeField] Image loader;
    IEnumerator Start()
    {
        float loadAmount = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            loadAmount += 0.05f;
            loader.fillAmount = loadAmount;
            if (loader.fillAmount == 1f)
            {
                MainMenuScreensController.instance.ShowScreen(MainMenuScreen.MainLobby);
                MainMenuScreensController.instance.DestroyScreen(MainMenuScreen.LoadingScreen);
                break;
            }
        }
    }

}
